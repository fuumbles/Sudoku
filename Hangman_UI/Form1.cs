using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Resources;
using System.Globalization;
using SudokuBiz;
using Hangman_Lib;

namespace Hangman_UI
{
    public partial class Form1 : Form
    {
        Biz db;
        public DateTime StartTime;
        public TimeSpan currTime;
        public string[,] PuzzleStrs = new string[1,3];
        public string UserPuzzle;
        string tmpstr; // Global Puzzle str without anything but the numbers
        string tempanswer; // Global puzzle answer without ','s 
        string tempUserName;

        ComponentResourceManager resources = new ComponentResourceManager(typeof(Form1));
        ResourceManager rm = new ResourceManager("WindowsFormsApplication11.Form1", typeof(Form1).Assembly);
        

        public Form1()
        {
            InitializeComponent();
            db = new Biz();
            tabPage2.Enabled = false;
            cbxPuzzleDiff.SelectedIndex = 0;//handling no selection for timer start
       
        }
        //--- Login Tab
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string[,] UserLogin = new string[1,2];
            if(tbPasswordLogin.Text.Length > 0 && tbUserNameLogin.Text.Length > 0)
            {
                //check for user
                tempUserName = tbUserNameLogin.Text.ToString();
                UserLogin = db.GetUserByUsernameAndPassword(tempUserName, tbPasswordLogin.Text.ToString());
                if(UserLogin[0,1] == "True")
                {

                    this.Text = "SuDoKu";//reset of text for appending the user name to it for personalization
                    MessageBox.Show("Welcome User: " + UserLogin[0, 0] + "!", "Successful Login");
                    this.Text = this.Text + " " + UserLogin[0, 0];
                    tabPage2.Enabled = true;

                }
                else
                {
                    MessageBox.Show("Please check your login information.","Incorrect Login");
                }
            }
            else
            {
                MessageBox.Show("Please enter a value to both Name and Password fields.", "Input Error");
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //localization
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                foreach (Control control in this.Controls)
                {
                    resources.ApplyResources(control, control.Name, Thread.CurrentThread.CurrentUICulture);
                }
                foreach (Control control in TabControl1.Controls)
                {
                    resources.ApplyResources(control, control.Name, Thread.CurrentThread.CurrentUICulture);
                }
                foreach (Control control in menuStrip1.Controls)
                {
                    resources.ApplyResources(control, control.Name, Thread.CurrentThread.CurrentUICulture);
                }
                foreach (ToolStripMenuItem control in menuStrip1.Items)
                {
                    resources.ApplyResources(control, control.Name, Thread.CurrentThread.CurrentUICulture);
                }
                foreach (ToolStripDropDownItem control in languageToolStripMenuItem.DropDownItems)
                {
                    resources.ApplyResources(control, control.Name, Thread.CurrentThread.CurrentUICulture);
                } 
                foreach (ToolStripDropDownItem control in quitToolStripMenuItem.DropDownItems)
                {
                    resources.ApplyResources(control, control.Name, Thread.CurrentThread.CurrentUICulture);
                }
                foreach (ToolStripDropDownItem control in changeToolStripMenuItem.DropDownItems)
                {
                    resources.ApplyResources(control, control.Name, Thread.CurrentThread.CurrentUICulture);
                }
                foreach (Control control in tabPage1.Controls)
                {
                    resources.ApplyResources(control, control.Name, Thread.CurrentThread.CurrentUICulture);
                }
                foreach (Control control in tabPage2.Controls)
                {
                    resources.ApplyResources(control, control.Name, Thread.CurrentThread.CurrentUICulture);
                }
                foreach (ToolStripMenuItem control in exitToolStripMenuItem.DropDownItems)
                {
                    resources.ApplyResources(control, control.Name, Thread.CurrentThread.CurrentUICulture);
                }

        }

        private void frenchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr-FR");
            foreach (Control control in this.Controls)
            {
                resources.ApplyResources(control, control.Name, Thread.CurrentThread.CurrentUICulture);
            }
            foreach (Control control in TabControl1.Controls)
            {
                resources.ApplyResources(control, control.Name, Thread.CurrentThread.CurrentUICulture);
            }
            foreach (ToolStripMenuItem control in menuStrip1.Items)
            {
                resources.ApplyResources(control, control.Name, Thread.CurrentThread.CurrentUICulture);
            }
            foreach (ToolStripDropDownItem control in languageToolStripMenuItem.DropDownItems)
            {
                resources.ApplyResources(control, control.Name, Thread.CurrentThread.CurrentUICulture);
            }
            foreach (ToolStripDropDownItem control in quitToolStripMenuItem.DropDownItems)
            {
                resources.ApplyResources(control, control.Name, Thread.CurrentThread.CurrentUICulture);
            }
            foreach (ToolStripDropDownItem control in changeToolStripMenuItem.DropDownItems)
            {
                resources.ApplyResources(control, control.Name, Thread.CurrentThread.CurrentUICulture);
            }
            foreach(Control control in tabPage1.Controls)
            {
                resources.ApplyResources(control, control.Name, Thread.CurrentThread.CurrentUICulture);
            }
            foreach (Control control in tabPage2.Controls)
            {
                resources.ApplyResources(control, control.Name, Thread.CurrentThread.CurrentUICulture);
            }
            foreach (ToolStripMenuItem control in exitToolStripMenuItem.DropDownItems)
            {
                resources.ApplyResources(control, control.Name, Thread.CurrentThread.CurrentUICulture);
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tbCreateUserName.Text.Length > 0 && tbCreateUserPass.Text.Length > 0)
            {
                string NewUserName = tbCreateUserName.Text.ToString();
                string NewPassword = tbCreateUserPass.Text.ToString();
                if (db.CreateUser(NewUserName, NewPassword))//creating user checks for existing in the biz class
                {
                    
                    MessageBox.Show("User successfully created!", "Successful Account Creation");
                    tbPasswordLogin.Text = NewPassword;
                    tbUserNameLogin.Text = NewUserName;
                    btnLogin.PerformClick();
                }
                else
                {
                    MessageBox.Show("User exists in database. Please try another name.", "Invalid Name");
                }
            }
            else
            {
                MessageBox.Show("Please enter a value to both Name and Password fields.", "Input Error");
            }
        }

        
        //--- End Login Tab


        //--- Game Tab
        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateTimer();
        }
        public void UpdateTimer()
        {
            currTime = DateTime.Now - StartTime;
            lblTimer.Text = currTime.ToString();
        }
        private void btnGetPuzz_Click(object sender, EventArgs e)
        {
            StartTime = DateTime.Now;
            timer1.Start();
            int DiffSel = cbxPuzzleDiff.SelectedIndex;
            PuzzleStrs = db.GetPuzzle(DiffSel);

            SetPuzzleValues(PuzzleStrs);
            
        }
        private void btnChkPuzz_Click(object sender, EventArgs e)
        {
            string CheckStr ="";
            if (timer1.Enabled)//will only submit one highscore since if 
                //the answer is right the timer is stopped and submitted
            {
                CheckStr += tb1.Text.ToString();
                CheckStr += tb2.Text.ToString();
                CheckStr += tb3.Text.ToString();
                CheckStr += tb4.Text.ToString();
                CheckStr += tb5.Text.ToString();
                CheckStr += tb6.Text.ToString();
                CheckStr += tb7.Text.ToString();
                CheckStr += tb8.Text.ToString();
                CheckStr += tb9.Text.ToString();
                CheckStr += tb10.Text.ToString();
                CheckStr += tb11.Text.ToString();
                CheckStr += tb12.Text.ToString();
                CheckStr += tb13.Text.ToString();
                CheckStr += tb14.Text.ToString();
                CheckStr += tb15.Text.ToString();
                CheckStr += tb16.Text.ToString();
                CheckStr += tb17.Text.ToString();
                CheckStr += tb18.Text.ToString();
                CheckStr += tb19.Text.ToString();
                CheckStr += tb20.Text.ToString();
                CheckStr += tb21.Text.ToString();
                CheckStr += tb22.Text.ToString();
                CheckStr += tb23.Text.ToString();
                CheckStr += tb24.Text.ToString();
                CheckStr += tb25.Text.ToString();
                CheckStr += tb26.Text.ToString();
                CheckStr += tb27.Text.ToString();
                CheckStr += tb28.Text.ToString();
                CheckStr += tb29.Text.ToString();
                CheckStr += tb30.Text.ToString();
                CheckStr += tb31.Text.ToString();
                CheckStr += tb32.Text.ToString();
                CheckStr += tb33.Text.ToString();
                CheckStr += tb34.Text.ToString();
                CheckStr += tb35.Text.ToString();
                CheckStr += tb36.Text.ToString();
                CheckStr += tb37.Text.ToString();
                CheckStr += tb38.Text.ToString();
                CheckStr += tb39.Text.ToString();
                CheckStr += tb40.Text.ToString();
                CheckStr += tb41.Text.ToString();
                CheckStr += tb42.Text.ToString();
                CheckStr += tb43.Text.ToString();
                CheckStr += tb44.Text.ToString();
                CheckStr += tb45.Text.ToString();
                CheckStr += tb46.Text.ToString();
                CheckStr += tb47.Text.ToString();
                CheckStr += tb48.Text.ToString();
                CheckStr += tb49.Text.ToString();
                CheckStr += tb50.Text.ToString();
                CheckStr += tb51.Text.ToString();
                CheckStr += tb52.Text.ToString();
                CheckStr += tb53.Text.ToString();
                CheckStr += tb54.Text.ToString();
                CheckStr += tb55.Text.ToString();
                CheckStr += tb56.Text.ToString();
                CheckStr += tb57.Text.ToString();
                CheckStr += tb58.Text.ToString();
                CheckStr += tb59.Text.ToString();
                CheckStr += tb60.Text.ToString();
                CheckStr += tb61.Text.ToString();
                CheckStr += tb62.Text.ToString();
                CheckStr += tb63.Text.ToString();
                CheckStr += tb64.Text.ToString();
                CheckStr += tb65.Text.ToString();
                CheckStr += tb66.Text.ToString();
                CheckStr += tb67.Text.ToString();
                CheckStr += tb68.Text.ToString();
                CheckStr += tb69.Text.ToString();
                CheckStr += tb70.Text.ToString();
                CheckStr += tb71.Text.ToString();
                CheckStr += tb72.Text.ToString();
                CheckStr += tb73.Text.ToString();
                CheckStr += tb74.Text.ToString();
                CheckStr += tb75.Text.ToString();
                CheckStr += tb76.Text.ToString();
                CheckStr += tb77.Text.ToString();
                CheckStr += tb78.Text.ToString();
                CheckStr += tb79.Text.ToString();
                CheckStr += tb80.Text.ToString();
                CheckStr += tb81.Text.ToString();
                if (CheckStr == tempanswer)//correct solution
                {
                    timer1.Stop();
                    //MessageBox.Show("Congratulations! You solved the Sudoku! Your score has been submitted to records.", "Correct Solution");
                    string completeTime = lblTimer.Text.ToString();
                    int Result = db.SubmitUserScore(tempUserName, completeTime, Convert.ToInt32(PuzzleStrs[0, 2]));
                    if (Result == 0)
                    {
                        MessageBox.Show("Score was not submitted correctly. Please try again.", "Error");
                    }
                    else if (Result == 1)
                    {
                        MessageBox.Show("Congratulations! You solved the Sudoku! Your score has been submitted to records.", "Correct Solution");
                    }
                    else if (Result == 2)
                    {
                        MessageBox.Show("Congratulations! You solved the Sudoku! Your score was quick enough to be added to the HighScores!", "Correct Solution");
                    }

                }
                else
                {
                    MessageBox.Show("Puzzle is not complete/correct, Please check your answer.", "Incorrect Solution");
                }
            }
            else
            {
                MessageBox.Show("Puzzle has not been started.", "Start the Puzzle");
            }
            
        }

        public void SetPuzzleValues(string[,] PuzzleVals)
        {
            //getting the incomplete and complete puzzle strings from the db and then parsing them
            //to get the values to be put into the textboxes
            string incPuzzle = PuzzleVals[0, 0];
            string cPuzzle = PuzzleVals[0, 1];
            int index;
            tmpstr = "";//reset
            tempanswer = ""; //reset
            for (index = 0; index < incPuzzle.Length; index++)
            {
                if (incPuzzle[index] != ',')
                {
                    tmpstr += incPuzzle[index];
                }
            }
            for (index = 0; index < cPuzzle.Length; index++)
            {
                if (cPuzzle[index] != ',')
                {
                    tempanswer += cPuzzle[index];
                }
            }

            //setting the text of all 81 goddamn text boxes with the new string from the stuff above
            SetText(tmpstr);
            
        }

        /// <summary>
        /// Pain in the ass - Holy shit
        /// </summary>
        /// <param name="PuzzleVal"></param>
        public void SetText(string PuzzleVal)
        {
            tb1.Text = PuzzleVal[0].ToString();
            tb2.Text = PuzzleVal[1].ToString();
            tb3.Text = PuzzleVal[2].ToString();
            tb4.Text = PuzzleVal[3].ToString();
            tb5.Text = PuzzleVal[4].ToString();
            tb6.Text = PuzzleVal[5].ToString();
            tb7.Text = PuzzleVal[6].ToString();
            tb8.Text = PuzzleVal[7].ToString();
            tb9.Text = PuzzleVal[8].ToString();
            tb10.Text = PuzzleVal[9].ToString();
            tb11.Text = PuzzleVal[10].ToString();
            tb12.Text = PuzzleVal[11].ToString();
            tb13.Text = PuzzleVal[12].ToString();
            tb14.Text = PuzzleVal[13].ToString();
            tb15.Text = PuzzleVal[14].ToString();
            tb16.Text = PuzzleVal[15].ToString();
            tb17.Text = PuzzleVal[16].ToString();
            tb18.Text = PuzzleVal[17].ToString();
            tb19.Text = PuzzleVal[18].ToString();
            tb20.Text = PuzzleVal[19].ToString();
            tb21.Text = PuzzleVal[20].ToString();
            tb22.Text = PuzzleVal[21].ToString();
            tb23.Text = PuzzleVal[22].ToString();
            tb24.Text = PuzzleVal[23].ToString();
            tb25.Text = PuzzleVal[24].ToString();
            tb26.Text = PuzzleVal[25].ToString();
            tb27.Text = PuzzleVal[26].ToString();
            tb28.Text = PuzzleVal[27].ToString();
            tb29.Text = PuzzleVal[28].ToString();
            tb30.Text = PuzzleVal[29].ToString();
            tb31.Text = PuzzleVal[30].ToString();
            tb32.Text = PuzzleVal[31].ToString();
            tb33.Text = PuzzleVal[32].ToString();
            tb34.Text = PuzzleVal[33].ToString();
            tb35.Text = PuzzleVal[34].ToString();
            tb36.Text = PuzzleVal[35].ToString();
            tb37.Text = PuzzleVal[36].ToString();
            tb38.Text = PuzzleVal[37].ToString();
            tb39.Text = PuzzleVal[38].ToString();
            tb40.Text = PuzzleVal[39].ToString();
            tb41.Text = PuzzleVal[40].ToString();
            tb42.Text = PuzzleVal[41].ToString();
            tb43.Text = PuzzleVal[42].ToString();
            tb44.Text = PuzzleVal[43].ToString();
            tb45.Text = PuzzleVal[44].ToString();
            tb46.Text = PuzzleVal[45].ToString();
            tb47.Text = PuzzleVal[46].ToString();
            tb48.Text = PuzzleVal[47].ToString();
            tb49.Text = PuzzleVal[48].ToString();
            tb50.Text = PuzzleVal[49].ToString();
            tb51.Text = PuzzleVal[50].ToString();
            tb52.Text = PuzzleVal[51].ToString();
            tb53.Text = PuzzleVal[52].ToString();
            tb54.Text = PuzzleVal[53].ToString();
            tb55.Text = PuzzleVal[54].ToString();
            tb56.Text = PuzzleVal[55].ToString();
            tb57.Text = PuzzleVal[56].ToString();
            tb58.Text = PuzzleVal[57].ToString();
            tb59.Text = PuzzleVal[58].ToString();
            tb60.Text = PuzzleVal[59].ToString();
            tb61.Text = PuzzleVal[60].ToString();
            tb62.Text = PuzzleVal[61].ToString();
            tb63.Text = PuzzleVal[62].ToString();
            tb64.Text = PuzzleVal[63].ToString();
            tb65.Text = PuzzleVal[64].ToString();
            tb66.Text = PuzzleVal[65].ToString();
            tb67.Text = PuzzleVal[66].ToString();
            tb68.Text = PuzzleVal[67].ToString();
            tb69.Text = PuzzleVal[68].ToString();
            tb70.Text = PuzzleVal[69].ToString();
            tb71.Text = PuzzleVal[70].ToString();
            tb72.Text = PuzzleVal[71].ToString();
            tb73.Text = PuzzleVal[72].ToString();
            tb74.Text = PuzzleVal[73].ToString();
            tb75.Text = PuzzleVal[74].ToString();
            tb76.Text = PuzzleVal[75].ToString();
            tb77.Text = PuzzleVal[76].ToString();
            tb78.Text = PuzzleVal[77].ToString();
            tb79.Text = PuzzleVal[78].ToString();
            tb80.Text = PuzzleVal[79].ToString();
            tb81.Text = PuzzleVal[80].ToString();
            

        }

        private void checkHighScoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Highscores> hscores = db.GetAllHighScores();

            //just checking if the DB isn't working
            if (hscores.Capacity == 0)
            {
                MessageBox.Show("No Highscores in records.", "Highscores Issue");
            }
            else                 
            {
                //formating is a bit wonky - the records are being saved and displayed properly
                MessageBox.Show("Name            Time\n" +
                                hscores[0].Username.ToString() + "         " + hscores[0].Time.ToString() + "\n"+
                                hscores[1].Username.ToString() + "         " + hscores[1].Time.ToString() + "\n" +
                                hscores[2].Username.ToString() + "         " + hscores[2].Time.ToString() + "\n" +
                                hscores[3].Username.ToString() + "         " + hscores[3].Time.ToString() + "\n" +
                                hscores[4].Username.ToString() + "         " + hscores[4].Time.ToString(), "Highscores!");
            }
        }


        //--- End Game Tab
    }
}
