using System;

public static class Pub
{
    //OnChange property containing all the 
    //list of subscribers callback methods
    public static event Action OnChange = delegate { };

    public static void Raise()
    {
        //Invoke OnChange Action
        OnChange();
    }
}