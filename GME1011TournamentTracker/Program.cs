namespace GME1011TournamentTracker
{
    internal class Program
    {
        /*
         * This program simulates a tournament manager that tracks participant names and scores. 
         * It uses 2x arrays - one to store the participant names and one to store the scores.
         * It sorts the arrays from high score to low score and prints somes stats.
         * It also picks a random winner for a prize draw - because, why not!?
         * 
         * The program was written by Aaron Langille - coordinator of GAME @ Cambrian.
         */
        static void Main(string[] args)
        {
            //call the PrintIntro method to print the initial welcome output.
            PrintIntro();

            //call GetNumParticipants to find out how many people participated in the tournament
            int numParticipants = GetNumParticipants();

            //set up our two arrays - one for names (string) and one for scores (int)
            Console.WriteLine("\nSetting up the tournament arrays...\n");
            int[] scores = new int[numParticipants];
            string[] names = new string[numParticipants];

            //prompt the user to get both the name and score of each participant.
            int currentParticipant = 0;
            while(currentParticipant < numParticipants)
            {
                Console.Write("Participant " + (currentParticipant + 1) + " name: ");
                names[currentParticipant] = Console.ReadLine();

                Console.Write("Participant " + (currentParticipant + 1) + " score: ");
                scores[currentParticipant] = int.Parse(Console.ReadLine());

                currentParticipant++;
            }

            //output and call the array sorting method - SortParticipantsByScore
            Console.WriteLine("\nSorting participants by score...\n");
            SortParticipantsByScore(scores, names);

            //print the results for the user
            Console.WriteLine("\nTournament Results...\n");
            for(int i = 0; i < names.Length; i++)
            {
                Console.WriteLine((i + 1) + ". " + names[i] + ": " + scores[i]);
            }


            //if the user made an input mistake, let's allow them to fix it.
            Console.WriteLine("\nIf you want to edit any entries, enter the line number (enter 0 if everything looks good): ");
            
            //what line number needs to be changed?
            int changeLineNumber = int.Parse(Console.ReadLine());
            
            //assume no lines will be changed
            bool entryChanged = false;
            
            //as long as the user wants to keep changing lines...
            while(changeLineNumber != 0)
            {
                //call the ChangeEntry method
                ChangeEntry(scores, names, changeLineNumber);
                
                //let them do it again (re-run the loop if needed)
                Console.WriteLine("\nIf you want to edit any entries, enter the line number (enter 0 if everything looks good): ");
                changeLineNumber = int.Parse(Console.ReadLine());
                
                //this flag is used below to re-sort and re-display the results if any lines have been changed.
                entryChanged = true;
            }

            //if the user changed an entry...
            if (entryChanged)
            {
                //...re-sort the array...
                Console.WriteLine("\nRe-sorting participants by score...\n");
                SortParticipantsByScore(scores, names);

                //...and redisplay the results.
                Console.WriteLine("\nUpdated Tournament Results...\n");
                for (int i = 0; i < names.Length; i++)
                {
                    Console.WriteLine((i + 1) + ". " + names[i] + ": " + scores[i]);
                }
            }

            //show of some simple stats
            DisplayTournamentStats(scores, names);

            //pick a random prize winner
            int prizeWinnerNumber = PickRandomPrizeWinner(scores.Length);
            CongratsPrizeWinner(names[prizeWinnerNumber]);

        }

        //these are the intro statements that get printed.
        public static void PrintIntro()
        {
            Console.WriteLine("Welcome to the GAMEBRIAN Tournament Tracker!");
            Console.WriteLine("--------------------------------------------\n");
        }

        //this is a method to prompt the user how many participants are in the tournament -
        //the method forces 4 or more to keep it interesting.
        public static int GetNumParticipants()
        {
            int numParticipants = 0;
            do
            {

                Console.Write("How many participants (at least 4)?: ");
                numParticipants = int.Parse(Console.ReadLine());

            } while (numParticipants < 4);

            return numParticipants;
        }


        //this is the most complex method in the program - it sorts the two arrays according to the score.
        public static void SortParticipantsByScore(int[] participantScores, string[] participantNames)
        {
            //this program uses a technique called parallel arrays. One array stores the name and the other stores the score.
            //the two arrays are "tied" by the index number. For example - scores array[0] holds the score for the participant
            //names in name array[0], scores array[1] holds the score for the participant names in name array[1] and so on...

            //if we sort the scores array by score value, we need to simultaneously move the names to the corresponding position.

            //we are going to start all entries in the score array, starting as 0.
            int startIndex = 0;

            //this makes sure that we go through all values so that the whole array is sorted
            while (startIndex < participantScores.Length)
            {
                //assume the biggest value left in the array is at our start position
                int biggestIndex = startIndex;

                //check the whole array and find the index of the biggest value remaining.
                for (int i = startIndex; i < participantScores.Length; i++)
                {
                     if (participantScores[i] > participantScores[biggestIndex])
                    {
                        biggestIndex = i;
                     }
                }

                //swap the starting value with the biggest value in the array
                int tempScore = participantScores[startIndex];
                participantScores[startIndex] = participantScores[biggestIndex];
                participantScores[biggestIndex] = tempScore;

                //swap the corresponding values in the names array
                string tempName = participantNames[startIndex];
                participantNames[startIndex] = participantNames[biggestIndex];
                participantNames[biggestIndex] = tempName;

                //move to the next value in the array and start the process again until 
                //all scores and corresponding names have been sorted.
                startIndex++;
            }
        }

        //if the user wants to change an entry, this method gets the job done.
        public static void ChangeEntry(int[] participantScores, string[] participantNames, int entryToChange)
        {
            int index = entryToChange - 1; //-1 because we're allowing the user to say entry 1 instead of entry 0
            Console.Write("Participant " + (index + 1) + " name: ");
            participantNames[index] = Console.ReadLine();

            Console.Write("Participant " + (index + 1) + " score: ");
            participantScores[index] = int.Parse(Console.ReadLine());
        }

        //some cheap stats work to show off who won the tournament, who came last, and what the average score was.
        public static void DisplayTournamentStats(int[] participantScores, string[] participantNames)
        {
            Console.WriteLine("\n\nTournament stats: ");
            Console.WriteLine("------------------");

            Console.WriteLine("Top score: " + participantNames[0] + " with " + participantScores[0]);
            Console.WriteLine("Low score: " + participantNames[participantNames.Length - 1] + " with " + participantScores[participantScores.Length - 1]);
            float averageScore = 0f;
            foreach(int score in participantScores)
            {
                averageScore += score;
            }
            Console.WriteLine("Average score based on " + participantScores.Length + " participants: " + averageScore / participantScores.Length);
        }

        //I like random things, so this picks a random participant number to give a prize to
        public static int PickRandomPrizeWinner(int numParticipants)
        {
            Random rng = new Random();
            return rng.Next(numParticipants);
        }

        //this congratulates them by name
        public static void CongratsPrizeWinner(string name)
        {
            Console.WriteLine("Congrats " + name + " - the winner of the random prize draw.");
        }
    }
}