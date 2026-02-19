using System;
using System.Collections.Generic;

namespace StateApp
{
    public class State
    {
        // Properties for each state object
        public string Name { get; set; }
        public string Code { get; set; }
        public int Population { get; set; }

        // Private array of states with state code and population
        private static string[,] statesData = new string[50, 3]
        {
            {"Alabama", "AL", "4903185"},
            {"Alaska", "AK", "731545"},
            {"Arizona", "AZ", "7278717"},
            {"Arkansas", "AR", "3017804"},
            {"California", "CA", "39538223"},
            {"Colorado", "CO", "5773714"},
            {"Connecticut", "CT", "3605944"},
            {"Delaware", "DE", "989948"},
            {"Florida", "FL", "21538187"},
            {"Georgia", "GA", "10711908"},
            {"Hawaii", "HI", "1456341"},
            {"Idaho", "ID", "1839106"},
            {"Illinois", "IL", "12812508"},
            {"Indiana", "IN", "6785528"},
            {"Iowa", "IA", "3190369"},
            {"Kansas", "KS", "2913314"},
            {"Kentucky", "KY", "4505836"},
            {"Louisiana", "LA", "4657757"},
            {"Maine", "ME", "1364212"},
            {"Maryland", "MD", "6177224"},
            {"Massachusetts", "MA", "7029917"},
            {"Michigan", "MI", "10077331"},
            {"Minnesota", "MN", "5706494"},
            {"Mississippi", "MS", "2961279"},
            {"Missouri", "MO", "6154913"},
            {"Montana", "MT", "1084225"},
            {"Nebraska", "NE", "1961504"},
            {"Nevada", "NV", "3104614"},
            {"New Hampshire", "NH", "1377529"},
            {"New Jersey", "NJ", "9288994"},
            {"New Mexico", "NM", "2117522"},
            {"New York", "NY", "20201249"},
            {"North Carolina", "NC", "10439388"},
            {"North Dakota", "ND", "779094"},
            {"Ohio", "OH", "11799448"},
            {"Oklahoma", "OK", "3956971"},
            {"Oregon", "OR", "4237256"},
            {"Pennsylvania", "PA", "13002700"},
            {"Rhode Island", "RI", "1097379"},
            {"South Carolina", "SC", "5118425"},
            {"South Dakota", "SD", "886667"},
            {"Tennessee", "TN", "6910840"},
            {"Texas", "TX", "29145505"},
            {"Utah", "UT", "3271616"},
            {"Vermont", "VT", "643503"},
            {"Virginia", "VA", "8631393"},
            {"Washington", "WA", "7705281"},
            {"West Virginia", "WV", "1793716"},
            {"Wisconsin", "WI", "5893718"},
            {"Wyoming", "WY", "576851"}
        };

        // Collection classes
        // Provides a list of State objects
        public static List<State> StateList { get; private set; }

        // Provides a dictionary of State names. The key is the State Code
        public static SortedDictionary<string, string> StatesDictionary { get; private set; }

        // Provides a sorted list of State objects. The key is the State Code
        public static SortedList<string, State> SortedStates { get; private set; }

        // Provides an integer list of state populations
        public static List<int> StatePops { get; private set; }

        // Constructor for creating State objects
        public State(string name, string code, int population)
        {
            Name = name;
            Code = code;
            Population = population;
        }

        // Static constructor to initialize all collections
        static State()
        {
            StateList = new List<State>();
            StatesDictionary = new SortedDictionary<string, string>();
            SortedStates = new SortedList<string, State>();
            StatePops = new List<int>();

            // Populate all collections from the statesData array
            for (int i = 0; i < statesData.GetLength(0); i++)
            {
                string name = statesData[i, 0];
                string code = statesData[i, 1];
                int population = int.Parse(statesData[i, 2]);

                // Create State object
                State state = new State(name, code, population);

                // Add to List
                StateList.Add(state);

                // Add to SortedDictionary (code -> name)
                StatesDictionary.Add(code, name);

                // Add to SortedList (code -> State object)
                SortedStates.Add(code, state);

                // Add population to List
                StatePops.Add(population);
            }
        }

        // Override ToString for easy display
        public override string ToString()
        {
            return $"{Name} ({Code}) - Population: {Population:N0}";
        }
    }
}
