using IQMStarterKit.Models.Core;

namespace IQMStarterKit.Models.Forms
{
    //page54
    public class ReviewQuizClass
    {
        // M1 (VARK, Personality Profiling)
        public string QuizAns1a { get; set; }
        public string QuizAns1a_Exp { get; set; }
        public string QuizAns1b { get; set; }
        public string QuizAns1b_Exp { get; set; }
        public string QuizAns1c { get; set; }
        public string QuizAns1c_Exp { get; set; }
        public string QuizAns1d { get; set; }
        public string QuizAns1d_Exp { get; set; }

        public string QuizAns2 { get; set; }
        public string QuizAns3 { get; set; }



        public string QuizAns4a { get; set; }
        public string QuizAns4b { get; set; }
        public string QuizAns4c { get; set; }
        public string QuizAns4d { get; set; }

        public string QuizAns5 { get; set; }
        public string QuizAns6 { get; set; }

        // M2 (Spirit of NZ, NZ My New Home)

        public string QuizAns7 { get; set; }
        public string QuizAns8 { get; set; }
        public string QuizAns9 { get; set; }
        public string QuizAns10 { get; set; }
        public string QuizAns11 { get; set; }
        public string QuizAns12 { get; set; }

        public string QuizAns13a { get; set; }
        public string QuizAns13b { get; set; }
        public string QuizAns13c { get; set; }
        public string QuizAns13d { get; set; }
        public string QuizAns13e { get; set; }
        public string QuizAns13f { get; set; }

        public string QuizAns14 { get; set; }
        public string QuizAns15 { get; set; }
        public string QuizAns16 { get; set; }

        // M3 (Be Proactive, Begin with the End in Mind)

        public string QuizAns17 { get; set; }
        public string QuizAns18 { get; set; }
        public string QuizAns19 { get; set; }
        public string QuizAns20 { get; set; }

        public string QuizAns21a { get; set; }
        public string QuizAns21b { get; set; }
        public string QuizAns21c { get; set; }
        public string QuizAns21d { get; set; }
        public string QuizAns21e { get; set; }

        // M4 (Put First Things First)

        public string QuizAns22 { get; set; }
        public string QuizAns23 { get; set; }

        // M5 (Think win win)
        public string QuizAns24 { get; set; }
        public string QuizAns25 { get; set; }

        public string QuizAns26a { get; set; }
        public string QuizAns26b { get; set; }
        public string QuizAns26c { get; set; }
        public string QuizAns26d { get; set; }

        public string QuizAns27 { get; set; }

        // M6 (Seek first)
        public string QuizAns28 { get; set; }

        public string QuizAns29a { get; set; }
        public string QuizAns29b { get; set; }
        public string QuizAns29c { get; set; }
        public string QuizAns29d { get; set; }
        public string QuizAns29e { get; set; }


        // M7 (Synergise)
        public string QuizAns30 { get; set; }
        public string QuizAns31 { get; set; }

        public string QuizAns32a { get; set; }
        public string QuizAns32b { get; set; }
        public string QuizAns32c { get; set; }
        public string QuizAns32d { get; set; }
        public string QuizAns32e { get; set; }

        public string QuizAns33a { get; set; }
        public string QuizAns33b { get; set; }
        public string QuizAns33c { get; set; }
        public string QuizAns33d { get; set; }
        public string QuizAns33e { get; set; }
        public string QuizAns33f { get; set; }

        public string QuizAns34 { get; set; }
        public string QuizAns35 { get; set; }
        public string QuizAns36 { get; set; }

        // M8 (Sharpen the saw) 
        public string QuizAns37 { get; set; }
        public string QuizAns38 { get; set; }
        public string QuizAns39 { get; set; }
        public string QuizAns40 { get; set; }


        public int TotalScore { get; set; }

        public StudentActivity StudentActivity { get; set; }

        public ReviewQuizClass()
        {
            StudentActivity = new StudentActivity();
        }

    }
}