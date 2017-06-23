using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Accord.Audio;
using Accord.Audio.Formats;
using Accord.DirectSound;
using Accord.Audio.Filters;
using Recorder.Recorder;
using Recorder.MFCC;
using System.Diagnostics;
using System.Collections.Generic;
using MetroFramework.Forms;


namespace Recorder
{
    /// <summary>
    ///   Speaker Identification application.
    /// </summary>
    /// 
    public partial class MainForm : MetroForm
    {
        /// <summary>
        ///   Calculates the distance between two audio frames in the form of an MFCCFrame object
        ///   returns the difference in a double
        /// </summary>
        /// 
        double distance(MFCCFrame frame1, MFCCFrame frame2)
        {
            double tempSum = 0;
            for (int i = 0; i < 13; i++)
            {
                double var1 = (Math.Abs(frame1.Features[i] - frame2.Features[i]));
                //tempSum += Math.Pow(Math.Abs(frame1.Features[i] - frame2.Features[i]), 2);
                tempSum += (var1 * var1);
            }
            return Math.Sqrt(tempSum);
        }

        /// <summary>
        ///   Improved DTW algorithm
        ///   Takes input 2 sequences: seq1 and seq2 to calculate the shortest distance between them
        ///   Uses the function "distance" defined above to calculate distance between two frames
        ///   use only 2 columns of rows length and calculate the first column and then the secoend column (by calculate distance and take the min from the first column),
        ///   the swap address of the 2 columns (make c1 point to c2 and c2 point to c1)to make the second column be the first column and the first column be the second
        ///   column and calculate it again ... 
        ///   Complixty O(rows*columns) <= not accurate analysis =>
        /// </summary>
        double DTW_improved(Sequence input, Sequence template)
        {
            // Don't compare two sequences if one of their lengths is half the other's
            if (input.Frames.Length <= (0.5 * template.Frames.Length) || template.Frames.Length <= (0.5 * input.Frames.Length))
                return double.PositiveInfinity;
            int rows = template.Frames.Length, columns = input.Frames.Length;

            double[] c1 = new double[rows];
            double[] c2 = new double[rows];
            double[] temp; // To hold address only (use it in swapping address) 
            c1[0] = distance(input.Frames[0], template.Frames[0]);
            for (int i = 1; i < rows; i++)
                c1[i] = c1[i - 1] + distance(input.Frames[0], template.Frames[i]);
            for (int i = 1; i < columns; i++)
            {
                c2[0] = distance(input.Frames[i], template.Frames[0]) + c1[0];
                c2[1] = distance(input.Frames[i], template.Frames[1]) + Math.Min(c1[0], c1[1]); // Calculating first 2 elements of the array before the loop
                for (int j = 2; j < rows; j++)
                    c2[j] = Math.Min(c1[j], Math.Min(c1[j - 1], c1[j - 2])) + distance(input.Frames[i], template.Frames[j]);

                if (i != columns - 1) // Swapping addresses of c1 & c2
                {
                    temp = c2;
                    c2 = c1;
                    c1 = temp;
                }
            }
            if(caseMilestone1.Checked) // Small testcases don't need normalization
                return c2[rows - 1];
            else
                return c2[rows - 1] / (0.5 * (input.Frames.Length + template.Frames.Length)); // Normalization: Dividing edit distance by average of input length & template length0
        }

        /// <summary>
        /// DTW Algotithm with Pruning
        /// the same concept of DTW above but we calculate only the window wich is around diagonal using W is the window length 
        /// Complixty O(rows*W)
        /// </summary>
        float Pruning_DTW(Sequence input, Sequence template)                                                                    //O(N * W)
        {
            // Don't compare two sequences if one of their lengths is half the other's
            if (input.Frames.Length <= (0.5 * template.Frames.Length) || template.Frames.Length <= (0.5 * input.Frames.Length)) //O(1)
                return float.PositiveInfinity;                                                                                  //O(1)        
            int rows = input.Frames.Length, columns = template.Frames.Length;                                                   //O(1)
            float cost;
            float[,] DTW = new float[rows + 1, columns + 1];                                                                    //O(1)
            int w = Math.Abs(columns - rows);// window length -> |rows - columns|<= w                                           //O(1)

            DTW[0, 0] = 0;
            for (int i = 1; i <= rows; i++)                                                                                     //O(N * W)
            {
                try { DTW[i, Math.Max(1, i - w) - 1] = float.PositiveInfinity;  
                    DTW[i, Math.Min(columns, i + w) + 1] = float.PositiveInfinity;  } catch { }                                 //O(1)
                for (int j = Math.Max(1, i - w); j <= Math.Min(columns, i + w); j++)                                            //O(W)
                {
                    cost = (float)distance(input.Frames[i - 1], template.Frames[j - 1]);                                        //O(1)
                    if (caseMilestone1.Checked) //Stretching/shrinking template doesn't produce correct output with small cases
                        DTW[i, j] = (cost + Math.Min(DTW[i - 1, j], Math.Min(DTW[i, j - 1], DTW[i - 1, j - 1])));               //O(1)
                    else {
                        if (j == Math.Max(1, i - w))                                                                            //O(1)
                            DTW[i, j] = (cost + Math.Min(DTW[i - 1, j], DTW[i - 1, j - 1]));
                        else                                                                                                    //O(1)
                            DTW[i, j] = (cost + Math.Min(DTW[i - 1, j], Math.Min(DTW[i - 1, j - 2], DTW[i - 1, j - 1])));
                    }
                }
            }
            return (float)(DTW[rows, columns] / (0.5 * (input.Frames.Length + template.Frames.Length)));                        //O(1)
        }

        /// <summary>
        /// Data of the opened audio file, contains:
        ///     1. signal data
        ///     2. sample rate
        ///     3. signal length in ms
        /// </summary>
        private AudioSignal signal = null;
        private string path;
        private Encoder encoder;
        private Decoder decoder;
        public string name;
        private bool isRecorded;

        /// <summary>
        ///  Struct relationItem that contains:
        ///      1. String name
        ///      2. String path
        ///      3. Difference between this person's sequence and the sequence that it's being compared to
        ///  Used in the btnIdentify_Click function
        /// </summary>
        public struct relationItem
        {
            public string nameStr;
            public string sequencePath;
            public double distance;
        }
        /// <summary>
        /// matchedPerson is the person who matched the wave which is opened, contain:
        ///     1.distance
        ///     2.name
        /// </summary>
        public struct matchedPerson
        {
            public string name;
            public double distance;
        }
        public MainForm()
        {
            InitializeComponent();

            // Configure the wavechart
            chart.SimpleMode = true;
            chart.AddWaveform("wave", Color.Green, 1, false);
            updateButtons();
        }

        /// <summary>
        ///   Starts recording audio from the sound card
        /// </summary>
        /// 
        private void btnRecord_Click(object sender, EventArgs e)
        {
            labelStatus.Text = "Status: Recording";
            isRecorded = true;
            this.encoder = new Encoder(source_NewFrame, source_AudioSourceError);
            this.encoder.Start();
            updateButtons();
        }

        /// <summary>
        ///   Plays the recorded audio stream.
        /// </summary>
        /// 
        private void btnPlay_Click(object sender, EventArgs e)
        {
            labelStatus.Text = "Status: Playing";
            InitializeDecoder();
            // Configure the track bar so the cursor
            // can show the proper current position
            if (trackBar1.Value < this.decoder.frames)
                this.decoder.Seek(trackBar1.Value);
            trackBar1.Maximum = this.decoder.samples;
            this.decoder.Start();
            updateButtons();
        }

        private void InitializeDecoder()
        {
            if (isRecorded)
            {
                // First, we rewind the stream
                this.encoder.stream.Seek(0, SeekOrigin.Begin);
                this.decoder = new Decoder(this.encoder.stream, this.Handle, output_AudioOutputError, output_FramePlayingStarted, output_NewFrameRequested, output_PlayingFinished);
            }
            else
            {
                this.decoder = new Decoder(this.path, this.Handle, output_AudioOutputError, output_FramePlayingStarted, output_NewFrameRequested, output_PlayingFinished);
            }
        }

        /// <summary>
        ///   Stops recording or playing a stream.
        /// </summary>
        /// 
        private void btnStop_Click(object sender, EventArgs e)
        {
            labelStatus.Text = "Status: Ready";
            Stop();
            updateButtons();
            updateWaveform(new float[BaseRecorder.FRAME_SIZE], BaseRecorder.FRAME_SIZE);
        }

        /// <summary>
        ///   This callback will be called when there is some error with the audio 
        ///   source. It can be used to route exceptions so they don't compromise 
        ///   the audio processing pipeline.
        /// </summary>
        /// 
        private void source_AudioSourceError(object sender, AudioSourceErrorEventArgs e)
        {
            throw new Exception(e.Description);
        }

        /// <summary>
        ///   This method will be called whenever there is a new input audio frame 
        ///   to be processed. This would be the case for samples arriving at the 
        ///   computer's microphone
        /// </summary>
        /// 
        private void source_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            this.encoder.addNewFrame(eventArgs.Signal);
            updateWaveform(this.encoder.current, eventArgs.Signal.Length);
        }

        /// <summary>
        ///   This event will be triggered as soon as the audio starts playing in the 
        ///   computer speakers. It can be used to update the UI and to notify that soon
        ///   we will be requesting additional frames.
        /// </summary>
        /// 
        private void output_FramePlayingStarted(object sender, PlayFrameEventArgs e)
        {
            updateTrackbar(e.FrameIndex);

            if (e.FrameIndex + e.Count < this.decoder.frames)
            {
                int previous = this.decoder.Position;
                decoder.Seek(e.FrameIndex);

                Signal s = this.decoder.Decode(e.Count);
                decoder.Seek(previous);

                updateWaveform(s.ToFloat(), s.Length);
            }
        }

        /// <summary>
        ///   This event will be triggered when the output device finishes
        ///   playing the audio stream. Again we can use it to update the UI.
        /// </summary>
        /// 
        private void output_PlayingFinished(object sender, EventArgs e)
        {
            updateButtons();
            updateWaveform(new float[BaseRecorder.FRAME_SIZE], BaseRecorder.FRAME_SIZE);
        }

        /// <summary>
        ///   This event is triggered when the sound card needs more samples to be
        ///   played. When this happens, we have to feed it additional frames so it
        ///   can continue playing.
        /// </summary>
        /// 
        private void output_NewFrameRequested(object sender, NewFrameRequestedEventArgs e)
        {
            this.decoder.FillNewFrame(e);
        }

        void output_AudioOutputError(object sender, AudioOutputErrorEventArgs e)
        {
            throw new Exception(e.Description);
        }

        /// <summary>
        ///   Updates the audio display in the wave chart
        /// </summary>
        /// 
        private void updateWaveform(float[] samples, int length)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    chart.UpdateWaveform("wave", samples, length);
                }));
            }
            else
            {
                if (this.encoder != null) { chart.UpdateWaveform("wave", this.encoder.current, length); }
            }
        }

        /// <summary>
        ///   Updates the current position at the trackbar.
        /// </summary>
        /// 
        private void updateTrackbar(int value)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    trackBar1.Value = Math.Max(trackBar1.Minimum, Math.Min(trackBar1.Maximum, value));
                }));
            }
            else
            {
                trackBar1.Value = Math.Max(trackBar1.Minimum, Math.Min(trackBar1.Maximum, value));
            }
        }

        private void updateButtons()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(updateButtons));
                return;
            }

            if (this.encoder != null && this.encoder.IsRunning())
            {
                btnAdd.Enabled = false;
                btnIdentify.Enabled = false;
                btnPlay.Enabled = false;
                btnStop.Enabled = true;
                btnRecord.Enabled = false;
                trackBar1.Enabled = false;
            }
            else if (this.decoder != null && this.decoder.IsRunning())
            {
                btnAdd.Enabled = false;
                btnIdentify.Enabled = false;
                btnPlay.Enabled = false;
                btnStop.Enabled = true;
                btnRecord.Enabled = false;
                trackBar1.Enabled = true;
            }
            else
            {
                /// <summary>
                ///  Disabling the buttons btnAdd and btnIdentify until an
                ///  audio file is opened
                /// </summary>
                /// 
                btnAdd.Enabled = this.path != null || this.encoder != null;
                btnIdentify.Enabled = false;
                btnPlay.Enabled = this.path != null || this.encoder != null; //stream != null;
                btnStop.Enabled = false;
                btnRecord.Enabled = true;
                trackBar1.Enabled = this.decoder != null;
                trackBar1.Value = 0;
            }

            btnIdentify.Enabled = true;
            btnAdd.Enabled = true;
        }

        private void MainFormFormClosed(object sender, FormClosedEventArgs e)
        {
            Stop();
        }

        private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.encoder != null)
            {
                Stream fileStream = saveFileDialog1.OpenFile();
                this.encoder.Save(fileStream);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            labelStatus.Text = "Status: Saving audio file...";
            saveFileDialog1.ShowDialog(this);
            labelStatus.Text = "Status: Ready";
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            if (this.encoder != null) { lbLength.Text = String.Format("Length: {0:00.00} sec.", this.encoder.duration / 1000.0); }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            labelStatus.Text = "Status: Opening audio file...";
            OpenFileDialog open = new OpenFileDialog();
            if (open.ShowDialog() == DialogResult.OK)
            {
                isRecorded = false;
                path = open.FileName;
                //Open the selected audio file
                signal = AudioOperations.OpenAudioFile(path);
                Sequence seq = AudioOperations.ExtractFeatures(signal);
                updateButtons();

                /// Enabling buttons Add and Identify
                /// The buttons btnIdentify and btnAdd are enabled ONLY after opening
                /// audio file
                ///            

            }
            /// <summary>
            ///  Updating status label
            /// </summary>
            /// 
            labelStatus.Text = "Status: Ready";
        }

        private void Stop()
        {
            if (this.encoder != null) { this.encoder.Stop(); }
            if (this.decoder != null) { this.decoder.Stop(); }
        }

        /// <summary>
        ///  Enable the interface objects used to get the speaker's name
        /// </summary>
        ///
        private void btnAdd_Click(object sender, EventArgs e)
        {
            /// <summary>
            ///  Disabling Add and Identify buttons
            /// </summary>
            ///
            btnIdentify.Enabled = false;
            btnAdd.Enabled = false;
            labelStatus.Text = "Status: Generating sequence and adding to database...";
            
           // To handle 3 testcases 
            List<User>[] Training = new List<User>[3];
            Training[0] = TestcaseLoader.LoadTestcase1Training(@"C:\SpeakerID\Test Cases\Complete SpeakerID Dataset\TrainingList.txt");
            Training[1] = TestcaseLoader.LoadTestcase2Training(@"C:\SpeakerID\Test Cases\Complete SpeakerID Dataset\TrainingList.txt");
            Training[2] = TestcaseLoader.LoadTestcase3Training(@"C:\SpeakerID\Test Cases\Complete SpeakerID Dataset\TrainingList.txt");
            for (int kk = 0; kk < 3; kk++)// To loop on all 3 testcases and write the sequences in the database
                for (int w = 0; w < Training[kk].Count; w++) // To loop on all users in each testcase
                {
                    for (int y = 0; y < Training[kk][w].UserTemplates.Count; y++) // To loop on all templates for each user
                    {
                        Sequence seqTemp = AudioOperations.ExtractFeatures(Training[kk][w].UserTemplates[y]);

                        /// <summary>
                        ///  Writes the sequence into the Database directory in C: drive -> C:/SpeakerID/Database
                        ///  Delimiter between each feature in one frame: %
                        ///  Delimiter between frames: #
                        /// </summary>
                        /// 
                        {
                            FileStream fs = new FileStream("C:/SpeakerID/case" + (kk + 1) + "/Database/" + Training[kk][w].UserName + y.ToString() + ".txt", FileMode.Create);
                            StreamWriter sw = new StreamWriter(fs);

                            /// <summary>
                            ///  For loop: converts a file from type "Sequence" -> seqTemp into a string that's ready to be written to a file
                            /// </summary>
                            ///
                            for (int i = 0; i < seqTemp.Frames.Length; i++)
                            {
                                for (int j = 0; j < 13; j++)
                                {
                                    sw.Write(seqTemp.Frames[i].Features[j]);
                                    if (j != 12) sw.Write('%');
                                }
                                if (i != seqTemp.Frames.Length - 1)
                                    sw.Write('#');
                            }
                            sw.Close();

                            /// <summary>
                            ///  Writes the relationship between speaker's name and the sequence file in the database, in the direcvtory -> C:/SpeakerID/Relathions.txt
                            ///  Delimiter between speaker's name and the path: %
                            ///  Delimiter between two relations: #
                            /// </summary>
                            ///
                            fs = new FileStream("C:/SpeakerID/case" + (kk + 1) + "/Relations.txt", FileMode.Append);
                            sw = new StreamWriter(fs);
                            sw.Write(Training[kk][w].UserName + '%' + "C:/SpeakerID/case" + (kk + 1) + "/Database/" + Training[kk][w].UserName + y.ToString() + ".txt" + '#');
                            sw.Close();
                        }
                    }
                }
            labelStatus.Text = "Status: Ready";

        }



        private void btnIdentify_Click(object sender, EventArgs e)
        {

            /// <summary>
            /// updating status label
            /// </summary>
            ///
            metroLabel1.Visible = true;
            pruningToggle1.Visible = true;
            labelStatus.Text = "Status: Identifying speaker and running DTW algorithm...";

           
            List<User> test = new List<User>();
            string cnt = "";// To choose a testcase 
            if (Testcase1_radioButton.Checked == true)
            { test = TestcaseLoader.LoadTestcase1Testing(@"C:\SpeakerID\Test Cases\Complete SpeakerID Dataset\TestingList.txt"); cnt = "1"; }
            else if (Testcase2_radioButton.Checked == true)
            { test = TestcaseLoader.LoadTestcase2Testing(@"C:\SpeakerID\Test Cases\Complete SpeakerID Dataset\TestingList.txt"); cnt = "2"; }
            else if (Testcase3_radioButton.Checked == true)
            { test = TestcaseLoader.LoadTestcase3Testing(@"C:\SpeakerID\Test Cases\Complete SpeakerID Dataset\TestingList.txt"); cnt = "3"; }
            else if(caseMilestone1.Checked) //First testcase
            {
                AudioSignal signalTemp = AudioOperations.OpenAudioFile("C:\\SpeakerID\\Test Cases\\Sample Test\\Input sample\\ItIsPlausible_Rich_US_English.wav");
                signalTemp = AudioOperations.RemoveSilence(signalTemp); //don't analyze (template function)
                User userTemp = new User();
                userTemp.UserTemplates = new List<AudioSignal>();
                userTemp.UserName = ("Rich");
                userTemp.UserTemplates.Add(signalTemp);
                test.Add(userTemp);
                cnt = "4";
            }
            else if (caseMilestone2.Checked)
            {
                AudioSignal signalTemp = AudioOperations.OpenAudioFile("C:\\SpeakerID\\Test Cases\\Pruning Test\\1 min\\[Input] Why Study Algorithms - (1 min).wav");
                signalTemp = AudioOperations.RemoveSilence(signalTemp);
                User userTemp = new User();
                userTemp.UserTemplates = new List<AudioSignal>();
                userTemp.UserName = ("BigOh (1 min)");
                userTemp.UserTemplates.Add(signalTemp);
                test.Add(userTemp);
                cnt = "5";
            }
            else if (caseMilestone3.Checked)
            {
                AudioSignal signalTemp = AudioOperations.OpenAudioFile("C:\\SpeakerID\\Test Cases\\Pruning Test\\4 min\\[Input] Why Study Algorithms - (4 min).wav");
                signalTemp = AudioOperations.RemoveSilence(signalTemp);
                User userTemp = new User();
                userTemp.UserTemplates = new List<AudioSignal>();
                userTemp.UserName = ("BigOh (4 min)");
                userTemp.UserTemplates.Add(signalTemp);
                test.Add(userTemp);
                cnt = "6";
            }
            for (int y = 0; y < test.Count; ++y)
            {
                Random rnd = new Random(); //Generate random number to choose an audio sample to test it against the database
                AudioSignal sig = new AudioSignal();
                sig = test[y].UserTemplates[0];//rnd.Next(0, test[y].UserTemplates.Count)];
                Sequence compareSequence_input = new Sequence();
                compareSequence_input = AudioOperations.ExtractFeatures(sig);
                FileStream FS = new FileStream("C:/SpeakerID/case" + cnt + "/Relations.txt", FileMode.Open);
                StreamReader SR = new StreamReader(FS);
                string relations = SR.ReadLine();
                SR.Close();
                /// <summary>
                ///  Split the string relations into an array of strings
                ///   Delimiter: #
                ///  Then split each string into name and path
                ///   Delimiter: %
                ///   Filling an array of "relationItem"s with the names and sequence paths of all sequences in the database, and leaving the
                ///   distance attribute in all the array items as they are "NULL", until calculated using the DTW algorithm
                /// </summary>
                /// 
                string[] relationsStrArr = relations.Split('#');
                relationItem[] relationsStructArr = new relationItem[relationsStrArr.Length - 1];
                matchedPerson person = new matchedPerson();
                person.distance = -1;

                for (int i = 0; i < relationsStructArr.Length; i++)
                {
                    string[] stringArrTemp = relationsStrArr[i].Split('%');
                    relationsStructArr[i].nameStr = stringArrTemp[0];
                    relationsStructArr[i].sequencePath = stringArrTemp[1];
                }
                Stopwatch sw;
                TimeSpan TS = TimeSpan.Zero;
                for (int i = 0; i < relationsStructArr.Length; i++) //Loops on all sequences in DB
                {
                    /// Reset the "compareSequence_output" variable and prepare SR for reading the sequence
                    Sequence compareSequence_output = new Sequence();
                    FS = new FileStream(relationsStructArr[i].sequencePath, FileMode.Open);
                    SR = new StreamReader(FS);
                    /// Resetting the "file" variable
                    string file = "";
                    file += SR.ReadLine();
                    string[] strTempFrames = file.Split('#'); //strTemp now contains all Frames in a sequence
                    compareSequence_output.Frames = new MFCCFrame[strTempFrames.Length]; //Setting number of frames in output sequence
                    for (int j = 0; j < strTempFrames.Length; j++) //Loops on all frames in a sequence
                    {
                        string[] strTempFeatures = strTempFrames[j].Split('%');
                        MFCCFrame frame = new MFCCFrame();
                        for (int k = 0; k < 13; k++) frame.Features[k] = double.Parse(strTempFeatures[k]); //Loops on all 13 features in a frame and converts string to double
                        compareSequence_output.Frames[j] = new MFCCFrame();
                        for (int k = 0; k < 13; k++)
                            compareSequence_output.Frames[j].Features[k] = frame.Features[k];
                    }

                    /// Save the difference between compareSequence_output and compareSequence_input in relationsStructArr[i].distance
                    if (pruningToggle1.Checked == true)
                    {
                        sw = Stopwatch.StartNew();
                        relationsStructArr[i].distance = Pruning_DTW(compareSequence_input, compareSequence_output);
                        sw.Stop();
                        TS += sw.Elapsed;
                    }
                    else
                    {
                        sw = Stopwatch.StartNew();
                        relationsStructArr[i].distance = DTW_improved(compareSequence_input, compareSequence_output);
                        sw.Stop();
                        TS += sw.Elapsed;
                    }

                    // update person object 
                    if (relationsStructArr[i].distance < person.distance || person.distance == -1)
                    {
                        person.distance = relationsStructArr[i].distance;
                        person.name = relationsStructArr[i].nameStr;
                    }
                    /// Close the SR
                    SR.Close();
                    Console.WriteLine(relationsStructArr[i].nameStr);
                }

                /// <summary>
                ///  Updating status label and showing search results
                /// </summary>
                ///
                labelStatus.Text = "Status: Searching done.";
                Console.WriteLine(test[y].UserName);
                MessageBox.Show("Search completed\nThe closest match to this speaker is: " + person.name + "\nTime Elapsed for DTW only: " + TS.Hours + ':' + TS.Minutes + ':' + TS.Seconds + ':' + TS.Milliseconds + "\nTotal cost: " + person.distance, "Search Results");
                labelStatus.Text = "Status: Ready";
            }
        }
    }
}