using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace StateApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            //declare variables
            int totalPopulation = 0;
            
            //Part 1
            // Create a new state object
            State PR = new State("*Puerto Rico*", "PR", 3200000);

            //Retrieve the List of states
            List<State> states = State.StateList;

            //Add the new State object to the List
            states.Add(PR);

            //Display all states in the List
            Console.WriteLine("List:");
            Console.WriteLine("{0,-20}{1,-15}{2}", "State Name", "State Code", "Population");
            foreach (State state in states)
            {
                Console.WriteLine("{0,-20}{1,-15}{2}", state.Name, state.Code, state.Population.ToString("N0"));
            }//foreach
            Console.WriteLine();
            //Remove the new State object from the List
            states.Remove(PR);

            //Part 2
            //Retrieve the StateDictionary
            SortedDictionary<string, string> stateDict = State.StatesDictionary;

            //Add the new State name to the Dictionary
            stateDict.Add(PR.Code, PR.Name);

            //Display each name in the Dictionary
            Console.WriteLine("Dictionary:");
            Console.WriteLine("{0,-20}{1}", "State Name", "State Code");
            foreach (KeyValuePair<string, string> kvp in stateDict)
            {
                Console.WriteLine("{0,-20}{1,-10}", kvp.Value, kvp.Key);
            }//foreach
            Console.WriteLine();

            //Remove the new State name from the Dictionary
            stateDict.Remove(PR.Code);

            //Part 3
            //Retrieve the SortedList
            SortedList<string, State> sortedStates = State.SortedStates;

            //Add the new State object to the SortedList
            sortedStates.Add(PR.Code, PR);

            //Display each State in the SortedList
            Console.WriteLine("SortedList:");
            Console.WriteLine("{0,-20}{1,-15}{2}", "State Name", "State Code", "Population");
            foreach (KeyValuePair<string, State> kvp in sortedStates)
            {
                State state = kvp.Value;
                Console.WriteLine("{0,-20}{1,-15}{2}", state.Name, state.Code, state.Population.ToString("N0"));
            }//foreach
            Console.WriteLine();

            //Remove the new State object from the SortedList
            sortedStates.Remove(PR.Code);

            //Part 4
            //Retrieve the StatePops list of population integers
            List<int> statePops = State.StatePops;

            //Add the population for the new state that was added to the list
            statePops.Add(PR.Population);

            //Display the sum of the state populations
            Console.WriteLine("Sum of StatePop:");
            foreach (int pop in statePops)
            {
                totalPopulation += pop;
            }//foreach
            Console.WriteLine("Total Population: " + totalPopulation.ToString("N0"));
        }//Main
    }//Program
}//Namespace