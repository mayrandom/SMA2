using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeTrust : MonoBehaviour
{

    public double[] gaussienne(double mean, double standardDeviation)
    {
        System.Random rand = new System.Random(1);
        double[] tab = new double[5];

        for (int i = 0; i < 5; i++)
        {
            double x;


            double u1 = 1.0 - rand.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0 - rand.NextDouble();
            double randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log((float) u1)) *
                         Mathf.Sin(2.0f * Mathf.PI * (float) u2); //random normal(0,1)
            x =
                        mean + standardDeviation * randStdNormal;

            if (x < 100)
                tab[i] = x;
            else i--;
        }

        return tab;
    }
}
