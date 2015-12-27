using System.Linq;
using UnityEngine;

public class localPlayerSelect : playerSelect
{

    public override void CheckInputs()
    {
        if (Input.GetButtonDown("kCancel") || Input.GetButtonDown("j1Cancel") || Input.GetButtonDown("j2Cancel") ||
            Input.GetButtonDown("j3Cancel") || Input.GetButtonDown("j4Cancel"))
        {
            if (kStage == SelectStages.Disabled && j1Stage == SelectStages.Disabled && j2Stage == SelectStages.Disabled && j3Stage == SelectStages.Disabled && j4Stage == SelectStages.Disabled)
            {
                Application.LoadLevel("Menu");
                return;
            }
        }

        base.CheckInputs();
        CheckSubmit();
    }

    public override void CheckCancel()
    {
        base.CheckCancel();
        if (Input.GetButton("kCancel"))
        {
            if (kCancel)
            {
                RemovePlayer("k");
                kCancel = false;
            }
        }
        if (Input.GetButton("j1Cancel"))
        {
            if (j1Cancel)
            {
                RemovePlayer("j1");
                j1Cancel = false;
            }
        }
        if (Input.GetButton("j2Cancel"))
        {
            if (j2Cancel)
            {
                RemovePlayer("j2");
                j2Cancel = false;
            }
        }
        if (Input.GetButton("j3Cancel"))
        {
            if (j3Cancel)
            {
                RemovePlayer("j3");
                j3Cancel = false;
            }
        }
        if (Input.GetButton("j4Cancel"))
        {
            if (j4Cancel)
            {
                RemovePlayer("j4");
                j4Cancel = false;
            }
        }
    }

    void CheckSubmit()
    {
        if (Input.GetButton("kStart"))
        {
            if (kSubmit)
            {
                string control = "k";
                if (kStage == SelectStages.Browse)
                {
                    if (pickedClasses.Count > 0 && pickedClasses.Contains(PlayerList.Find(c => c.Control == control).Class))
                    {
                        PlayPlayersAudio(InvalidClip, control);
                        return;
                    }

                    AddPlayer(control);
                    kStage = SelectStages.Chosen;
                    kCanHorizontal = false;
                }
                if (kStage == SelectStages.Disabled)
                {
                    SelectInitialPlayer(control);
                    kStage = SelectStages.Browse;
                    kCanHorizontal = true;
                }
                kSubmit = false;

            }
        }
        if (Input.GetButton("j1Start"))
        {
            if (j1Submit)
            {
                string control = "j1";

                if (j1Stage == SelectStages.Browse)
                {
                    if (pickedClasses.Count > 0 && pickedClasses.Contains(PlayerList.Find(c => c.Control == control).Class))
                    {
                        PlayPlayersAudio(InvalidClip, control);
                        return;
                    }
                    AddPlayer(control);
                    j1Stage = SelectStages.Chosen;
                    j1CanHorizontal = false;
                }
                if (j1Stage == SelectStages.Disabled)
                {
                    SelectInitialPlayer(control);
                    j1Stage = SelectStages.Browse;
                    j1CanHorizontal = true;
                }
                j1Submit = false;

            }

        }
        if (Input.GetButton("j2Start"))
        {
            if (j2Submit)
            {
                string control = "j2";

                if (j2Stage == SelectStages.Browse)
                {
                    if (pickedClasses.Count > 0 && pickedClasses.Contains(PlayerList.Find(c => c.Control == control).Class))
                    {
                        PlayPlayersAudio(InvalidClip, control);
                        return;
                    }

                    AddPlayer(control);
                    j2Stage = SelectStages.Chosen;
                    j2CanHorizontal = false;
                }
                if (j2Stage == SelectStages.Disabled)
                {
                    SelectInitialPlayer(control);
                    j2Stage = SelectStages.Browse;
                    j2CanHorizontal = true;
                }
                j2Submit = false;

            }

        }
        if (Input.GetButton("j3Start"))
        {
            if (j3Submit)
            {
                string control = "j3";

                if (j3Stage == SelectStages.Browse)
                {
                    if (pickedClasses.Count > 0 && pickedClasses.Contains(PlayerList.Find(c => c.Control == control).Class))
                    {
                        PlayPlayersAudio(InvalidClip, control);
                        return;
                    }

                    AddPlayer(control);
                    j3Stage = SelectStages.Chosen;
                    j3CanHorizontal = false;
                }
                if (j3Stage == SelectStages.Disabled)
                {
                    SelectInitialPlayer(control);
                    j3Stage = SelectStages.Browse;
                    j3CanHorizontal = true;
                }
                j3Submit = false;

            }

        }
        if (Input.GetButton("j4Start"))
        {
            if (j4Submit)
            {
                string control = "j4";

                if (j4Stage == SelectStages.Browse)
                {
                    if (pickedClasses.Count > 0 && pickedClasses.Contains(PlayerList.Find(c => c.Control == control).Class))
                    {
                        PlayPlayersAudio(InvalidClip, control);
                        return;
                    }

                    AddPlayer(control);
                    j4Stage = SelectStages.Chosen;
                    j4CanHorizontal = false;
                }
                if (j4Stage == SelectStages.Disabled)
                {
                    SelectInitialPlayer(control);
                    j4Stage = SelectStages.Browse;
                    j4CanHorizontal = true;
                }
                j4Submit = false;

            }

        }
    }


}
