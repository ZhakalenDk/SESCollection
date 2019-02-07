#region pre-script
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using VRageMath;
using VRage.Game;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Ingame;
using Sandbox.Game.EntityComponents;
using VRage.Game.Components;
using VRage.Collections;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI.Ingame;

namespace TerminalNamingRule
{
    public class Program : MyGridProgram
    {
        #endregion
        //To put your code in a PB copy from this comment...

        #region Comment
        /*---------------------------------------------------------------------------------------HOW TO USE---------------------------------------------------------------------------*/
        /*
                <About the script>
                    Info:
                        The script will Rename everything on the current grid following the syntax of; [Type] <[Info]> - [Room] ([Grid Name])
                        Example: Connecter <Rover Bay> - Garage (Earth Base)
                        Everything that already has the grid name in the name will not be targeted.


                <Set Up Script>
                    Info:
                        The script will collect all blocks on the current grid.
                        If you use tags on the PB itself remember NOT to but the tag at the end.
                        For best result place the tag in the start of the custom data.
                        (Don't worry. The script will leave the tag out, when renaming termninal blocks)
                    
                    <How to>
                        Inside the custom data on the PB, you can write down what the script should put in as [Room] and [Info].
                        Syntax is as follows: [Info],[Room].
                        Exmaple: Rover Bay,Garage
                        Output: Connecter <Rover Bay> - Garage (Earth Base).
                        Earth Base is the name of the grid in this scenario.

                <Tags>
                    Info:
                        Tags can be used to bypass the scripts behavior
                        
                    Tags:
                        [1] - ¤i - Put this tag inside the custom data of any block you don't want the script to rename
                    
                        

                <Error Handling>
                    The script will indicate if it's running by showing an animation in the PB's message area.
                    If it not animating the script is not running.
                    If this is the case try and do every step again. Something might have gone wrong in the process.

                    Otherwise look at the error message in the PB's message are. It will tell you exactly what the problem is.
                    In the case the error message does not update to "None" even though every mistake has been corrected -> Recompile the script.
        */
        /*------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        #endregion

        #region CONSTANTS
        #endregion

        /*------------------------------------------------------------------------------------------SCRIPT--------------------------------------------------------------------------------*/
        readonly RunIndicator AnimationRunner = new RunIndicator ();

        public Program ()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update1;  //  UPdates itself every game tick.
        }

        public void Save ()
        {

        }

        readonly List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock> ();

        public void Main ( string argument, UpdateType updateType )
        {
            string [] naming = Me.CustomData.Split ( ',' );

            GridTerminalSystem.GetBlocks ( this.blocks );

            for ( int i = 0; i < this.blocks.Count; i++ )
            {
                if ( this.blocks [i].CubeGrid == Me.CubeGrid && !this.blocks [i].CustomName.Contains ( Get_Grid_Name () ) )
                {
                    if ( !this.blocks [i].CustomData.Contains ( "¤i" ) )
                    {
                        this.blocks [i].CustomName = Create_Name ( this.blocks [i] );
                    }
                }

            }


            Echo ( this.AnimationRunner.Run_Animation () );
        }

        /// <summary>
        /// Return the name of the grid
        /// </summary>
        /// <returns></returns>
        private string Get_Grid_Name ()
        {
            return Me.CubeGrid.CustomName;
        }

        /// <summary>
        /// Get the information to fill the formating
        /// </summary>
        /// <returns></returns>
        private string [] Get_Info ()
        {
            if ( Me.CustomData.Length > 0 ) //  If the custom data of the PB is not empty
            {
                string [] info = Me.CustomData.Split ( ',' );

                //  Remove the tag if there is one
                if ( info [0].Contains ( "¤i" ) )   //  If the tag is in the first part
                {
                    info [0] = Me.CustomData.Remove ( Me.CustomData.IndexOf ( '¤' ), Me.CustomData.IndexOf ( 'i' ) + 1 );
                }
                else if ( info [1].Contains ( "¤i" ) )  //  If the tag is in the second part
                {
                    info [1] = Me.CustomData.Remove ( Me.CustomData.IndexOf ( '¤' ), Me.CustomData.IndexOf ( 'i' ) - 1 );
                }

                return info;
            }

            return new string [2];
        }

        /// <summary>
        /// Get the type of the block
        /// </summary>
        /// <param name="_block"></param>
        /// <returns></returns>
        private string Get_Block_Type ( IMyTerminalBlock _block )
        {
            string type = string.Empty;

            if ( _block.BlockDefinition.SubtypeName != Me.BlockDefinition.SubtypeName ) //  If It's not a progammable block
            {
                if ( _block.DetailedInfo.Length > 0 && _block.DetailedInfo.Contains ( "Type" ) )    //  If the detailed info contains the right circumstances
                {
                    type = _block.DetailedInfo.Split ( '\n' ) [0].Split ( ':' ) [1].Trim ();
                }
                else
                {
                    if ( _block.BlockDefinition.SubtypeName == string.Empty )   //  For special occasion where blocks have an empty string as Subtype (Thanks Malware)
                    {
                        if ( _block is IMyDoor )
                        {
                            type = "Door";

                            if ( _block is IMyAirtightDoorBase )
                            {
                                type = "Hangar Door";
                            }

                        }

                        else if ( _block is IMyLargeGatlingTurret )
                        {
                            type = "Gattling Turret";
                        }
                        else if ( _block is IMyLargeMissileTurret )
                        {
                            type = "Missile Turret";
                        }
                    }

                }
            }
            else
            {
                type = "Programmable block";
            }

            return type;
        }

        /// <summary>
        /// Generate Name string
        /// </summary>
        /// <param name="_block"></param>
        /// <returns></returns>
        private string Create_Name ( IMyTerminalBlock _block )
        {
            string name = $"{Get_Block_Type ( _block )} {( ( Get_Info () [0] != string.Empty ) ? $"<{Get_Info () [0]}>" : ( string.Empty ) )} - {Get_Info () [1]} ({Get_Grid_Name ()})";

            return name;
        }

        /// <summary>
        /// Provides an animation on wether or not the program is running
        /// </summary>
        class RunIndicator
        {
            private int runCount = 0;
            private string animation = string.Empty;
            public string ErrorText { private get; set; }
            public int ErrorFLAG { get; set; }

            public RunIndicator ()
            {
                ErrorText = string.Empty;
                ErrorFLAG = 0;
            }

            /// <summary>
            /// Indication of the scrip running
            /// </summary>
            private void Run_Indicator ()
            {
                if ( this.runCount == 8 )
                {
                    this.animation = ( $"|======-||-======|\nErrors: {ErrorText}" );
                }
                if ( this.runCount == 16 )
                {
                    this.animation = ( $"|=====-=||=-=====|\nErrors: {ErrorText}" );
                }
                if ( this.runCount == 24 )
                {
                    this.animation = ( $"|====-==||==-====|\nErrors: {ErrorText}" );
                }
                if ( this.runCount == 32 )
                {
                    this.animation = ( $"|==-====||===-===|\nErrors: {ErrorText}" );
                }
                if ( this.runCount == 40 )
                {
                    this.animation = ( $"|==-====||====-==|\nErrors: {ErrorText}" );
                }
                if ( this.runCount == 48 )
                {
                    this.animation = ( $"|=-=====||=====-=|\nErrors: {ErrorText}" );
                }
                if ( this.runCount == 56 )
                {
                    this.animation = ( $"|-======||======-|\nErrors: {ErrorText}" );
                }
                if ( this.runCount == 64 )
                {
                    this.animation = ( $"|=-=====||=====-=|\nErrors: {ErrorText}" );
                }
                if ( this.runCount == 72 )
                {
                    this.animation = ( $"|==-====||====-==|\nErrors: {ErrorText}" );
                }
                if ( this.runCount == 80 )
                {
                    this.animation = ( $"|===-===||===-===|\nErrors: {ErrorText}" );
                }
                if ( this.runCount == 88 )
                {
                    this.animation = ( $"|====-==||==-====|\nErrors: {ErrorText}" );
                }
                if ( this.runCount == 96 )
                {
                    this.runCount = 0;
                    this.animation = ( $"|=====-=||=-=====|\nErrors: {ErrorText}" );
                }
            }

            /// <summary>
            /// Runs animation
            /// </summary>
            /// <returns></returns>
            public string Run_Animation ()
            {
                Run_Indicator ();
                this.runCount++;
                return this.animation;
            }
        }
        //to this comment.
        #region post-script
    }
}
#endregion