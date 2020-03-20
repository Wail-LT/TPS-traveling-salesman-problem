using PTS.App.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTS.App.SelectionMetodes
{
    public class Tournament : SelectionMethode
    {
        public new const double D_MUTATE_FACTOR = 0.01;
        public Tournament(double mFactor = D_MUTATE_FACTOR) : base(mFactor) { }
        
        //Needed for reflexion
        public Tournament() : base(D_MUTATE_FACTOR) { }
        public override Route Selection(List<Route> routes)
        {
            List<int> participant = new List<int>();

            //Create Tournament between 15% of the routes
            int size = (int)(routes.Count * 0.15);

            Random random = Utils.Utils.Random;

            //First participant
            int index = random.Next(0, routes.Count);
            //Add it to the list of participant
            participant.Add(index);

            //Index of new participants 
            int rIndex;

            for (int i = 0; i < size - 1; i++)
            {
                do
                {
                    //New random participant
                    rIndex = random.Next(0, routes.Count);
                } while (participant.Contains(rIndex));

                //Add it to the list of participant
                participant.Add(rIndex);

                //If fitess of the new participant is better than the current best one
                //then save it as better participant
                if (routes[index].Fitness > routes[rIndex].Fitness)
                {
                    index = rIndex;
                }
            }

            return routes[index];
        }
    }
}
