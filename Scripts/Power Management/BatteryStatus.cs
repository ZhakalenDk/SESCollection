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

namespace BatteryStatus
{
    public class Program : MyGridProgram
    {
        #endregion
        //To put your code in a PB copy from this comment...

        #region Comment
        /*---------------------------------------------------------------------------------------HOW TO USE---------------------------------------------------------------------------*/
        /*
                <Set up scripts>
                    Step 1:
                            You can change the values of the region CONSTANTS below.
                            The LCD_TAG is the tag used to ID the screens you want the script to use (Default is: <Battery>).

                            You can also change the color of the screen by changing the value of SCREEN_COLOR (Default is: Green).

                <Setting up LCD's>
                    Step 1:
                            All LCD's that should be included by the script must have <LCD_TAG> in their name.
                            Example: LCD <Battery>.
                    
                    Step 2 (Optional):
                            Some screen have different sizes, which can lead to unwanted behavior.
                            YOu can modify this by using tags. (See below under <Formatting tags>).

                            All you have to do is write the tag somewhere in the name of the screen you want the tag to be used on.
                            Example: %# LCD <Battery> %sl (This will change the screen to display every battery with graphical representation of procentage).

                <Setting up batteries>
                    Step 1:
                            The script wil automatically collect every battery on the grid. (Will not collect batteries on any subgrid)
                            This it. Nothing more to do, the script will work fine now.
                    
                    Step 2 (Optional):
                            You can force screens to display either graphical representation of, or normal procentage by including tags (See below under <Formatting tags>).
                            Example: %# Battery.

                            Even if a screen is set to display normal procentage (100.00%) it will display that specific battery with graphical representation.

                    Step 3 (Optional)
                            You can change the displayed name of any battery by going into Custom Data in the terminal view.
                            Here you can write any name you want, as long as it's less that 7 characters long.
                            This will now be the name displayed on the screen.
                            Exmaple: Inside custom name I write 1234567. Output = [001] 1234567   :   [100.00%].

                <Formatting tags>
                    Info  :
                            Tags define how the LCD should display the information given to it.
                            Below you will see a list of all tags available and what they do.
                    Tags  :
                            [1] - %#  - Used on screens and batteries to force the formatting into visual representation of percentage
                            [2] - %%  - Used on batteries to force the formatting into plain procentage
                            [3] - %s  - Used on screens to force the formatting into displaying only 4 batteries at a time (Useful for flat LCD's)
                            [4] - %sl - Used on screens to force the formatting into displaying only one batteries at a time (useful for large grid flat and corner LCD's)
                            [5] - %w  - used on screens to force the display to show in widescreen 

                <Error Handling>
                    The script will indicate if it's running by showing an animation in the PB's message area.
                    If it not animating the script is not running.
                    If this is the case try and do every step again. Something might have gone wrong in the process.

                    Otherwise look at the error message in the PB's message are. It will tell you exactly what the problem is.
                    In the case the error message does not update to "None" even though every mistake has been corrected -> Recompile the script or contact me (Pied Piper)
        */
        /*------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
        #endregion

        #region CONSTANTS
        const string LCD_TAG = "<Battery>";
        Color SCREEN_COLOR = new Color ( 0, 255, 0 );
        #endregion

        /*------------------------------------------------------------------------------------------SCRIPT--------------------------------------------------------------------------------*/
        readonly RunIndicator AnimationRunner = new RunIndicator ();

        List<IMyBatteryBlock> G_Batteries = new List<IMyBatteryBlock> ();
        int G_TickCounter = 0;

        public Program ()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update1;  //  UPdates itself every game tick.
        }

        public void Save ()
        {

        }

        public void Main ( string argument, UpdateType updateType )
        {
            Echo ( this.AnimationRunner.Run_Animation () );
            this.AnimationRunner.ErrorText = "None";

            this.G_TickCounter++;

            Set_Up_LCDs ( Get_LCD () );

            if ( this.AnimationRunner.ErrorFLAG == 2 )
            {
                this.AnimationRunner.ErrorText = "Battery error";
            }

        }

        /// <summary>
        /// Get all LCD's on current grid
        /// </summary>
        /// <returns></returns>
        private List<IMyTextPanel> Get_LCD ()
        {
            List<IMyTextPanel> LCDs = new List<IMyTextPanel> ();

            try
            {
                GridTerminalSystem.GetBlocksOfType ( LCDs, item => ( ( item.CustomName.Contains ( LCD_TAG ) ) && item.CubeGrid.CustomName == Get_Grid_Name () ) );
                return LCDs;
            }
            catch ( Exception )
            {

                throw;
            }
        }

        /// <summary>
        /// Set up LCD screens
        /// </summary>
        /// <param name="_LCDs">The list of LCD's to configure</param>
        private void Set_Up_LCDs ( List<IMyTextPanel> _LCDs )
        {
            for ( int i = 0; i < _LCDs.Count; i++ )
            {
                _LCDs [i].ShowPublicTextOnScreen ();
                _LCDs [i].FontColor = this.SCREEN_COLOR;

                _LCDs [i].FontSize = 0.7320f;
                _LCDs [i].Font = "Monospace";

                Print_Status ( _LCDs [i] );
            }
        }

        /// <summary>
        /// Print the status of battries to LCDs
        /// </summary>
        private void Print_Status ( IMyTextPanel _LCD )
        {
            bool useGraphics = false;
            bool isWideScreen = false;
            if ( _LCD.CustomData.Contains ( "%#" ) )
            {
                useGraphics = true;
            }

            if ( _LCD.CustomData.Contains ( "%w" ) )
            {
                isWideScreen = true;
            }

            int allowedLines = ( ( _LCD.CustomData.Contains ( "%s" ) ) ? ( ( ( _LCD.CustomData.Contains ( "%sl" ) ) ? ( 1 ) : ( 4 ) ) ) : ( 22 ) );

            _LCD.WritePublicText ( Battery_Status ( Get_Batteries (), useGraphics, allowedLines, isWideScreen ) );
        }

        /// <summary>
        /// Get the grid name
        /// </summary>
        /// <returns></returns>
        private string Get_Grid_Name ()
        {
            return Me.CubeGrid.CustomName;  //  Me referes to the block this script is executed in. CubeGrid is the grid the block is attached to and CustomName is the name of the grid.
        }

        /// <summary>
        /// Get all batteries on the current grid
        /// </summary>
        /// <returns></returns>
        private List<IMyBatteryBlock> Get_Batteries ()
        {
            List<IMyBatteryBlock> batteries = new List<IMyBatteryBlock> ();

            try
            {
                GridTerminalSystem.GetBlocksOfType ( batteries, item => item.CubeGrid.CustomName == Get_Grid_Name () ); //  Get blocks of type IMyBattery but only on the current grid

                if ( batteries.Count != this.G_Batteries.Count )    //  Used for scrolling
                {
                    this.G_Batteries = batteries;
                }

                return batteries;
            }
            catch ( Exception )
            {
                throw;
            }
        }

        /// <summary>
        /// Return a string containing details on all batteries in the current grid
        /// </summary>
        /// <param name="_batteries">The list of batteries on this grid</param>
        /// <param name="_useGraphics">Wether or not to render a ghrafical representation of the percentage</param>
        /// <param name="_amountOfLinesAllowed">The amount of lines that are allowed ot be printed</param>
        /// <returns></returns>
        private string Battery_Status ( List<IMyBatteryBlock> _batteries, bool _useGraphics, int _amountOfLinesAllowed, bool _isWideScreen )
        {
            #region Display prototype
            /*
                |----------[Battery Status]----------| 
                | [001] [Battery]   :   [##########] |
                | [002] [Battery]   :   [##########] |
                | [003] [Battery]   :   [##########] |
                | [004] [Battery]   :   [##########] |
                | [005] [Battery]   :   [##########] |
                | [006] [Battery]   :   [##########] |
                | [007] [Battery]   :   [##########] |
                | [008] [Battery]   :   [##########] |
                | [009] [Battery]   :   [##########] |
                | [010] [Battery]   :   [##########] |
                | [011] [Battery]   :   [##########] |
                | [012] [Battery]   :   [##########] |
                | [013] [Battery]   :   [##########] |
                | [014] [Battery]   :   [##########] |
                | [015] [Battery]   :   [##########] |
                | [016] [Battery]   :   [##########] |
                | [017] [Battery]   :   [##########] |
                | [018] [Battery]   :   [#####-----] |
                | [019] [Battery]   :   [########--] |
                | [020] [Battery]   :   [########--] |
                | [021] [Battery]   :   [#---------] |
                | [022] [Battery]   :   [#---------] |
                |-------------[#########]------------|
                
                |---------[Battery Status]---------| 
                |       No Batteries on grid       |
                |                                  |
                |                                  |
                |                                  |
                |                                  |
                |                                  |
                |                                  |
                |                                  |
                |                                  |
                |                                  |
                |                                  |
                |                                  |
                |                                  |
                |                                  |
                |                                  |
                |                                  |
                |                                  |
                |                                  |
                |                                  |
                |                                  |
                |                                  |
                |-------------[100.00]-------------|

                |---------[Battery Status]---------| 
                | [001] Battery   :-------[000.00] |
                | [002] Battery   :-------[000.00] |
                | [003] Battery   :-------[000.00] |
                | [004] Battery   :-------[000.00] |
                | [001] Battery   :-------[100.00] |
                |-------------[100.00]-------------|
            */
            #endregion

            float overAllMax = 0;
            float overAllCurrent = 0;
            float overAllPercentage = 0;

            //int LineNumber = 0;


            string formatString = $"|{CreateFormat ( ( ( _isWideScreen ) ? ( 27 ) : ( 9 ) ), '-' )}[Battery Status]{CreateFormat ( ( ( _isWideScreen ) ? ( 27 ) : ( 9 ) ), '-' )}|\n" /*string.Empty*/;

            //  If the current screen allows less lines than there's batteries in the list. Execute this.
            if ( _batteries.Count > _amountOfLinesAllowed )
            {
                if ( this.G_TickCounter >= 100 )    //  Execute this every 100 gae trick
                {
                    this.G_TickCounter = 0;

                    Scroll_Page ();
                }
            }

            if ( _batteries.Count != 0 )    //  Only execute this if there's batteries on the grid
            {
                for ( int i = 0, j = 21; i < ( ( _batteries.Count > _amountOfLinesAllowed ) ? ( _amountOfLinesAllowed ) : ( _batteries.Count ) ); i++ )
                {
                    string digits = string.Empty;
                    if ( i < 100 )  //  If 'i' is less than 100, add a '0' in front of the string
                    {
                        if ( ( i + 1 ) < 10 )  //  If 'i' is less than 10, add a '0' in front of the string
                        {
                            digits += '0';
                        }

                        digits += '0';
                    }

                    /*                                            If less batteries than allowed lines            use this              else use this        bool for graphic     */
                    string leftSide = Formatted_Battery_String ( ( ( _batteries.Count < _amountOfLinesAllowed ) ? ( _batteries [i] ) : ( this.G_Batteries [i] ) ), _useGraphics );
                    string rightSide = string.Empty;
                    if ( _isWideScreen && i > 21 )
                    {
                        rightSide = $"[{digits}{j + 1}]{Formatted_Battery_String ( ( ( _batteries.Count < _amountOfLinesAllowed ) ? ( _batteries [j] ) : ( this.G_Batteries [j] ) ), _useGraphics )}";
                        j++;
                    }
                    else if ( _isWideScreen && i < 21 )
                    {
                        rightSide = $"{CreateFormat ( 33, ' ' )} |";
                    }



                    formatString += $"| [{digits}{i + 1}] {leftSide} | {rightSide}\n";    //  The actual information we want to send out

                    overAllMax += _batteries [i].MaxStoredPower;
                    overAllCurrent += _batteries [i].CurrentStoredPower;
                }

                formatString += $"{Blank_Lines ( ( _amountOfLinesAllowed - _batteries.Count ), _isWideScreen )}";

                overAllPercentage = ( overAllCurrent / overAllMax ) * 100;

            }
            else    //  If theres no batteries, execute this part
            {
                formatString += "|       No Batteries on grid       |\n";
                formatString += Blank_Lines ( ( _amountOfLinesAllowed - 1 ), _isWideScreen );
            }



            string endStringFormat = $"{CreateFormat ( ( ( _isWideScreen ) ? ( 29 ) : ( 11 ) ), '-' )}";

            /*                  if graphic is prefere                              use this                                                                     else use this                                 */
            formatString += $"|{( ( _useGraphics ) ? ( $"{endStringFormat}[{Render_Graphics ( overAllPercentage )}]{endStringFormat}" ) : ( $"--{endStringFormat}[{overAllPercentage.ToString ( "000.00" )}]--{endStringFormat}" ) )}|";

            return formatString;
        }

        private void Insert_Procentage ( IMyBatteryBlock _battery, float _percentage )
        {
            if ( _battery.CustomName.Contains("<") )
            {
                string start = _battery.CustomName.Split ( '<' ) [0];
                string end = _battery.CustomName.Split ( '>' ) [1];
                _battery.CustomName = $"{start}<{_percentage.ToString ( "F" )}%>{end}";
            }
            else
            {
                _battery.CustomName = "Battery <>";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_length">The length of the sequence</param>
        /// <param name="_symbol">THe symbol to print</param>
        /// <returns></returns>
        private string CreateFormat ( int _length, char _symbol )
        {
            string format = string.Empty;
            for ( int i = 0; i < _length; i++ )
            {
                format += _symbol;
            }

            return format;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int Line_Number ()
        {
            return 0;
        }

        /// <summary>
        /// Scroll trough the page if there's to many lines
        /// </summary>
        private void Scroll_Page ()
        {
            IMyBatteryBlock battery = this.G_Batteries [0]; //  Refrence to the first element in the list
            this.G_Batteries.Remove ( this.G_Batteries [0] );   //  Remove that element from the list
            this.G_Batteries.Add ( battery );   //  Add it back into the list
        }

        /// <summary>
        /// Returns a string with x amount of blank lines
        /// </summary>
        /// <param name="_amount">How many lines should be returned</param>
        /// <returns></returns>
        private string Blank_Lines ( int _amount, bool _isWidescreen )
        {
            string blankLines = string.Empty;
            for ( int i = 0; i < _amount; i++ )
            {
                if ( _isWidescreen )
                {
                    blankLines += "|                                  |                                  |\n";
                }
                else
                {
                    blankLines += "|                                  |\n";
                }

            }

            return blankLines;
        }

        /// <summary>
        /// Return a formatted string containing the status of a battery
        /// </summary>
        /// <param name="_battery">The battery to extract details from</param>
        /// <param name="_useGraphic">Wether or not to show percentage as a float</param>
        /// <returns></returns>
        private string Formatted_Battery_String ( IMyBatteryBlock _battery, bool _useGraphics )
        {
            if ( _battery.CustomData.Contains ( "%#" ) )
            {
                _useGraphics = true;
            }
            else if ( _battery.CustomData.Contains ( "%%" ) )
            {
                _useGraphics = false;
            }

            float maxStored = _battery.MaxStoredPower;
            float currentStored = _battery.CurrentStoredPower;

            /*50 / 100 = 0.5f * 100 = 50%*/

            float percentage = ( currentStored / maxStored ) * 100;

            Insert_Procentage ( _battery, percentage );

            return $"{( ( _battery.CustomName.Length > 7 ) ? ( _battery.CustomName.Remove ( 7, ( _battery.CustomName.Length - 7 ) ) ) : ( _battery.CustomName ) )}{CreateFormat ( ( 7 - _battery.CustomName.Length ), ' ' )}   :{( ( _useGraphics ) ? ( $"   [{Render_Graphics ( percentage )}]" ) : ( $"-------[{percentage.ToString ( "000.00" )}]" ) )}";
        }

        /// <summary>
        /// Create the visual representation of percentage
        /// </summary>
        /// <param name="_percentage">the percentage to visualize</param>
        /// <returns></returns>
        private string Render_Graphics ( float _percentage )
        {
            string graphic = string.Empty;
            for ( int i = 0; i < 10; i++ )  //  Loop 10 times because each 10% is represented as a '#'.
            {
                if ( i < ( _percentage / 10 ) ) //  as long as 'i' is less than procentage devided by 10 print a '#'. [Example: 20 / 10 = 2 = "##"]
                {
                    graphic += '#';

                }
                else    //  Fill the rest of the string with a dash.
                {
                    graphic += "-";
                }
            }

            return graphic;

        }

        /// <summary>
        /// Provides an animation on wether or not the program is running
        /// </summary>
        class RunIndicator
        {
            private int runCount = 0;
            private string animation = string.Empty;
            /// <summary>
            /// message to write [Example: "No message to show"]
            /// </summary>
            public string ErrorText { private get; set; }
            /// <summary>
            /// Raise a FLAG when an error accoures. [Example: ErrorFLAG = 1]
            /// </summary>
            public int ErrorFLAG { get; set; }

            /// <summary>
            /// Constructer. Creates and instance of this object.  [Example: RunIndicator animation = new RunIndicator ();]
            /// </summary>
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