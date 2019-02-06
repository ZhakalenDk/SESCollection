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

namespace TRM
{
    public class Program : MyGridProgram
    {
        #endregion
        //To put your code in a PB copy from this comment...

        #region Comment
        /*---------------------------------------------------------------------------------------HOW TO USE---------------------------------------------------------------------------*/
        /*
                <>
                    Step 1:

                <>
                    Step 1:

                <>
                    Step 1:

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
        const string GRID_NAME = "None";
        const string LCD_TAG = "<Gas>";
        Color SCREEN_COLOR = new Color ( 0, 255, 0 );
        #endregion

        /*------------------------------------------------------------------------------------------SCRIPT--------------------------------------------------------------------------------*/

        public Program ()
        {
            //Runtime.UpdateFrequency = UpdateFrequency.Update1;  //  UPdates itself every game tick.
        }

        public void Save ()
        {

        }

        public void Main ( string argument, UpdateType updateType )
        {
            Interpreter.GridName = Me.CubeGrid.CustomName;
            string code = Get_Custom_Data ();

            List<IMyTerminalBlock> allBlocks = new List<IMyTerminalBlock> ();
            GridTerminalSystem.GetBlocks ( allBlocks );



            if ( code.Length > 0 )
            {
                Interpreter.Interprete ( code );

                Rename_Block ( allBlocks );
            }
        }

        private void Rename_Block ( List<IMyTerminalBlock> _allBlocks )
        {
            for ( int i = 0; i < _allBlocks.Count; i++ )
            {
                if ( _allBlocks [i] is IMyAirtightDoorBase )
                {
                    Echo ( $"Renaming: {_allBlocks [i].CustomName}" );
                    _allBlocks [i].CustomName = Interpreter.Blocks.Find ( item => item.Type == "hangarDoor" ).BlockString ();
                }
                else if ( _allBlocks [i] is IMyAirtightSlideDoor )
                {
                    Echo ( $"Renaming: {_allBlocks [i].CustomName}" );
                    _allBlocks [i].CustomName = Interpreter.Blocks.Find ( item => item.Type == "slidingDoor" ).BlockString ();
                }
                else if ( _allBlocks [i] is IMyAirVent )
                {
                    Echo ( $"Renaming: {_allBlocks [i].CustomName}" );
                    _allBlocks [i].CustomName = Interpreter.Blocks.Find ( item => item.Type == "vent" ).BlockString ();
                }
                else if ( _allBlocks [i] is IMyArtificialMassBlock )
                {
                    Echo ( $"Renaming: {_allBlocks [i].CustomName}" );
                    _allBlocks [i].CustomName = Interpreter.Blocks.Find ( item => item.Type == "massBlock" ).BlockString ();
                }
                else if ( _allBlocks [i] is IMyAssembler )
                {
                    Echo ( $"Renaming: {_allBlocks [i].CustomName}" );
                    _allBlocks [i].CustomName = Interpreter.Blocks.Find ( item => item.Type == "assembler" ).BlockString ();
                }
                else if ( _allBlocks [i] is IMyBatteryBlock )
                {
                    Echo ( $"Renaming: {_allBlocks [i].CustomName}" );
                    _allBlocks [i].CustomName = Interpreter.Blocks.Find ( item => item.Type == "battery" ).BlockString ();
                }
                else if ( _allBlocks [i] is IMyBeacon )
                {
                    Echo ( $"Renaming: {_allBlocks [i].CustomName}" );
                    _allBlocks [i].CustomName = Interpreter.Blocks.Find ( item => item.Type == "beacon" ).BlockString ();
                }
                else if ( _allBlocks [i] is IMyButtonPanel )
                {
                    Echo ( $"Renaming: {_allBlocks [i].CustomName}" );
                    _allBlocks [i].CustomName = Interpreter.Blocks.Find ( item => item.Type == "buttenPanel" ).BlockString ();
                }
                else if ( _allBlocks [i] is IMyCameraBlock )
                {
                    Echo ( $"Renaming: {_allBlocks [i].CustomName}" );
                    _allBlocks [i].CustomName = Interpreter.Blocks.Find ( item => item.Type == "camera" ).BlockString ();
                }
                else if ( _allBlocks [i] is IMyCargoContainer )
                {
                    Echo ( $"Renaming: {_allBlocks [i].CustomName}" );
                    _allBlocks [i].CustomName = Interpreter.Blocks.Find ( item => item.Type == "container" ).BlockString ();
                }
                else if ( _allBlocks [i] is IMyCockpit )
                {
                    Echo ( $"Renaming: {_allBlocks [i].CustomName}" );
                    _allBlocks [i].CustomName = Interpreter.Blocks.Find ( item => item.Type == "cockpit" ).BlockString ();
                }
                else if ( _allBlocks [i] is IMyCollector )
                {
                    Echo ( $"Renaming: {_allBlocks [i].CustomName}" );
                    _allBlocks [i].CustomName = Interpreter.Blocks.Find ( item => item.Type == "collector" ).BlockString ();
                }
                else if ( _allBlocks [i] is IMyControlPanel )
                {
                    Echo ( $"Renaming: {_allBlocks [i].CustomName}" );
                    _allBlocks [i].CustomName = Interpreter.Blocks.Find ( item => item.Type == "controlPanel" ).BlockString ();
                }
                else if ( _allBlocks [i] is IMyConveyorSorter )
                {
                    Echo ( $"Renaming: {_allBlocks [i].CustomName}" );
                    _allBlocks [i].CustomName = Interpreter.Blocks.Find ( item => item.Type == "sorter" ).BlockString ();
                }
                else if ( _allBlocks [i] is IMyCryoChamber )
                {
                    Echo ( $"Renaming: {_allBlocks [i].CustomName}" );
                    _allBlocks [i].CustomName = Interpreter.Blocks.Find ( item => item.Type == "cryopod" ).BlockString ();
                }
                else if ( _allBlocks [i] is IMyDecoy )
                {
                    Echo ( $"Renaming: {_allBlocks [i].CustomName}" );
                    _allBlocks [i].CustomName = Interpreter.Blocks.Find ( item => item.Type == "decoy" ).BlockString ();
                }
                else if ( _allBlocks [i] is IMyDoor )
                {
                    Echo ( $"Renaming: {_allBlocks [i].CustomName}" );
                    _allBlocks [i].CustomName = Interpreter.Blocks.Find ( item => item.Type == "door" ).BlockString ();
                }
                else if ( _allBlocks [i] is IMyGasGenerator )
                {
                    Echo ( $"Renaming: {_allBlocks [i].CustomName}" );
                    _allBlocks [i].CustomName = Interpreter.Blocks.Find ( item => item.Type == "gasGen" ).BlockString ();
                }
                else if ( _allBlocks [i] is IMyGasTank )
                {
                    Echo ( $"Renaming: {_allBlocks [i].CustomName}" );
                    if ( _allBlocks [i].DetailedInfo.Split ( '\n' ) [0].Remove ( 0, 4 ) == "Hydrogen Tank" )
                    {
                        _allBlocks [i].CustomName = Interpreter.Blocks.Find ( item => item.Type == "h2Tank" ).BlockString ();
                    }
                    else if ( _allBlocks [i].DetailedInfo.Split ( '\n' ) [0].Remove ( 0, 4 ) == "Oxygen Tank" )
                    {
                        _allBlocks [i].CustomName = Interpreter.Blocks.Find ( item => item.Type == "o2Tank" ).BlockString ();
                    }
                }
                else if ( _allBlocks [i] is IMyGravityGenerator )
                {
                    Interpreter.BlockDetails block = Interpreter.Blocks.Find ( item => item.Type == "gravGen" );
                    Echo ( $"Renaming: {_allBlocks [i].CustomName}" );
                    _allBlocks [i].CustomName = block.BlockString ();
                }
                else if ( _allBlocks [i] is IMyGyro )
                {
                    Echo ( $"Renaming: {_allBlocks [i].CustomName}" );
                    _allBlocks [i].CustomName = Interpreter.Blocks.Find ( item => item.Type == "gyro" ).BlockString ();
                }
                else if ( _allBlocks [i] is IMyInteriorLight )
                {
                    Echo ( $"Renaming: {_allBlocks [i].CustomName}" );
                    _allBlocks [i].CustomName = Interpreter.Blocks.Find ( item => item.Type == "interiorLight" ).BlockString ();
                }
                else if ( _allBlocks [i] is IMyJumpDrive )
                {
                    Echo ( $"Renaming: {_allBlocks [i].CustomName}" );
                    _allBlocks [i].CustomName = Interpreter.Blocks.Find ( item => item.Type == "jumpDrive" ).BlockString ();
                }
                else if ( _allBlocks [i] is IMyLandingGear )
                {
                    Echo ( $"Renaming: {_allBlocks [i].CustomName}" );
                    _allBlocks [i].CustomName = Interpreter.Blocks.Find ( item => item.Type == "gear" ).BlockString ();
                }
                else if ( _allBlocks [i] is IMyMedicalRoom )
                {
                    Echo ( $"Renaming: {_allBlocks [i].CustomName}" );
                    _allBlocks [i].CustomName = Interpreter.Blocks.Find ( item => item.Type == "medbay" ).BlockString ();
                }
                else if ( _allBlocks [i] is IMyOreDetector )
                {
                    Echo ( $"Renaming: {_allBlocks [i].CustomName}" );
                    _allBlocks [i].CustomName = Interpreter.Blocks.Find ( item => item.Type == "oreDetector" ).BlockString ();
                }
                else if ( _allBlocks [i] is IMyProgrammableBlock )
                {
                    Echo ( $"Renaming: {_allBlocks [i].CustomName}" );
                    _allBlocks [i].CustomName = Interpreter.Blocks.Find ( item => item.Type == "program" ).BlockString ();
                }
                else if ( _allBlocks [i] is IMyReactor )
                {
                    Echo ( $"Renaming: {_allBlocks [i].CustomName}" );
                    _allBlocks [i].CustomName = Interpreter.Blocks.Find ( item => item.Type == "reactor" ).BlockString ();
                }
                else if ( _allBlocks [i] is IMyRefinery )
                {
                    Echo ( $"Renaming: {_allBlocks [i].CustomName}" );
                    _allBlocks [i].CustomName = Interpreter.Blocks.Find ( item => item.Type == "refinery" ).BlockString ();
                }
                else if ( _allBlocks [i] is IMyRemoteControl )
                {
                    Echo ( $"Renaming: {_allBlocks [i].CustomName}" );
                    _allBlocks [i].CustomName = Interpreter.Blocks.Find ( item => item.Type == "remote" ).BlockString ();
                }
                else if ( _allBlocks [i] is IMySensorBlock )
                {
                    Echo ( $"Renaming: {_allBlocks [i].CustomName}" );
                    _allBlocks [i].CustomName = Interpreter.Blocks.Find ( item => item.Type == "sensor" ).BlockString ();
                }
                else if ( _allBlocks [i] is IMyShipConnector )
                {
                    Echo ( $"Renaming: {_allBlocks [i].CustomName}" );
                    _allBlocks [i].CustomName = Interpreter.Blocks.Find ( item => item.Type == "connector" ).BlockString ();
                }
                else
                {
                    continue;
                }
            }
        }

        private string Get_Custom_Data ()
        {
            return Me.CustomData;
        }

        static class Interpreter
        {
            public static int NumberOfBlocks { get; private set; }
            public static List<BlockDetails> Blocks = new List<BlockDetails> ();
            private static string G_Name;
            private static string G_Info;
            private static string G_Room;
            private static string G_GridName;
            public static string GridName { get; set; }

            //  Find wrapping values
            private static string [] [] G_WrappingValues = { new string [2], new string [2], new string [2], new string [2] };

            // Find order values
            private static string [] G_OrderValues = { "", "", "", "", "" };
            private static int [] G_Order = { 0, 0, 0, 0 };

            /// <summary>
            /// Interprete code
            /// </summary>
            /// <param name="_code"></param>
            /// <returns></returns>
            public static string [] Interprete ( string _code )
            {
                #region Explanation
                /* 
                        'syntax' should represent the format of the block renaming. (To indicate that the syntax formatting is over close off the code block with '/'. Exmaple: <syntax>Code goes here</syntax>)
                            Everything folled by an '=' (equal sign) is the value.
                            Each statement is closed of by adding a new line. So every statment has to be on it's own line. Two statements can't share the same line.
                            If there is no 'naming' code block for a type the block type will not be renamed in the terminal

                        'naming' should represent the naming of each block. (To indicate that the naming formatting is over close off the code block with '/'. Exmaple: <naming=blockToFormat>Code goes here</naming>)
                            Which block Type to name, using the specified format should be determed by the keyword followed by the equal sign. (Example: <naming=container>code goes here</naming>)


                         keywords:
                        'order' refers to the order in which you want the format to be.
                        'nameWrap', 'infoWrap', 'roomWrap' and 'gridWrap' is supposed to tell which symbols you want the specified word wrappen in. (Exmaple: nameTag=* will wrap the name of the block in: *Block Name*)
                        ',' (Comma) is used to seperate the value of a tag into beginning symbol and ending symbol. (Example: nameTag=*,>. This will result in *MyNewBlockName>)
                        'name', 'info', 'room' and 'gridname' indicate the specified keywords for; NameOfBlock, InformationOfBlock, RoomOfBlock, NameOfGrid
                        'name' represents the custom name of the block. (Example: name=MyNewBlockName)
                        'info' represents the custom info of the block (Optional). (Example: info=Not used.)
                        'room' represents the room, which the block is placed in (Optional). (Exmaple: room=Storage Room)
                        'gridname' represents the grid the block is placed on (can be a custom name or the actual grid). (Example: gridname=This is a grid)
                        'this' refers to the actual grid name. (Example: If the name of the acutal grid is Earth Base then 'this' will return Earth Base.)
                        'empty' refers to the value being empty. (Example: nametag=empty. Will result in no symbol wraping for the name of the block)




                    <syntax>
                        order="nameinfo,room,gridName"
                        nameWrap=*,*
                        infoWrap=<,>
                        roomWrap=-,empty
                        gridWrap=(
                     </syntax>
                    <naming=container>
                        name=Container
                        infoTag=Iron
                        room=Storage
                        gridName=this
                    </naming>
                    <naming=Battery>
                        name=Battery
                        info=100%
                        room=PowerBank
                        gridName=this
                    </naming>


                    Should output: *name* <info> - room (gridname)
                    The container blocks in the terminal should now be called: *Container* <Iron> - storage (Earth Base)
                    And
                    The Battery blocks in the terminal should now be called: *Battery* <100%> - Power Bank (Earth Base)
                 */
                #endregion

                char [] codeTokens = { '=' };
                string [] codeKeywords = { "syntax", "name", "info", "room", "gridName", "this", "naming", "order", "nameWrap", "infoWrap", "roomWrap", "gridWrap", "empty" };

                string [] linesOfCode = Seperate_into_lines ( _code );

                Read_Blocks ( linesOfCode );


                return null;
            }

            /// <summary>
            /// Seperate code string into lines
            /// </summary>
            /// <param name="_code"></param>
            /// <returns></returns>
            private static string [] Seperate_into_lines ( string _code )
            {
                return _code.Split ( '\n' );
            }

            /// <summary>
            /// Seperate code string into blocks of code
            /// </summary>
            /// <param name="_lines"></param>
            private static void Read_Blocks ( string [] _lines )
            {
                string tokenSubValue = string.Empty;
                for ( int i = 0; i < _lines.Length; i++ )
                {
                    if ( Get_Token ( _lines [i] ) [0] == "format" || Get_Token ( _lines [i] ) [0] == "naming" )
                    {

                        tokenSubValue = Get_Token ( _lines [i] ) [1];

                        for ( int j = i; j < _lines.Length; j++, i++ )
                        {
                            if ( Get_Token ( _lines [j] ) [0] == "/format" || Get_Token ( _lines [j] ) [0] == "/naming" )
                            {
                                if ( Get_Token ( _lines [j] ) [0] == "/naming" )
                                {
                                    BlockDetails block = new BlockDetails ( tokenSubValue, G_Name, G_Info, G_Room, G_GridName );
                                    Blocks.Add ( block );
                                    NumberOfBlocks++;
                                }

                                break;
                            }


                            if ( _lines [j].Contains ( "=" ) )
                            {
                                Execute ( _lines [j] );
                            }

                        }
                    }
                }
            }

            /// <summary>
            /// Execute a line of code
            /// </summary>
            /// <param name="_statement"></param>
            private static void Execute ( string _statement )
            {
                string [] statementSplit = _statement.Split ( '=' );

                //  Statement values
                string token = statementSplit [0];
                string value = statementSplit [1];


                switch ( token )
                {
                    case "order":
                        G_Order = Find_Order ( value );
                        break;
                    case "nameWrap":
                        G_WrappingValues [0] = Find_Wrapping ( value );
                        break;
                    case "infoWrap":
                        G_WrappingValues [1] = Find_Wrapping ( value );
                        break;
                    case "roomWrap":
                        G_WrappingValues [2] = Find_Wrapping ( value );
                        break;
                    case "gridWrap":
                        G_WrappingValues [3] = Find_Wrapping ( value );
                        break;
                    case "name":
                        G_Name = Eva_Value ( value );
                        break;
                    case "info":
                        G_Info = Eva_Value ( value );
                        break;
                    case "room":
                        G_Room = Eva_Value ( value );
                        break;
                    case "gridName":
                        G_GridName = Eva_Value ( value );
                        break;
                    default:
                        break;
                }
            }

            /// <summary>
            /// Evaluate value of a line
            /// </summary>
            /// <param name="_value"></param>
            /// <returns></returns>
            private static string Eva_Value ( string _value )
            {
                if ( _value == "empty" )
                {
                    return string.Empty;
                }
                else if ( _value == "this" )
                {
                    return GridName;
                }

                return _value;
            }

            /// <summary>
            /// Find the order of the name,info,room and grid tags
            /// </summary>
            /// <param name="_value">The value with the order line</param>
            /// <returns></returns>
            private static int [] Find_Order ( string _value )
            {
                int [] orderValues = { 0, 0, 0, 0, };   //  Values for order
                string [] execute = _value.Split ( ',' );   //  Split each keyword into an array
                for ( int i = 0; i < orderValues.Length; i++ )
                {
                    string check = execute [i];
                    switch ( check )
                    {
                        case "name":
                            orderValues [i] = 1;
                            break;
                        case "info":
                            orderValues [i] = 2;
                            break;
                        case "room":
                            orderValues [i] = 3;
                            break;
                        case "gridName":
                            orderValues [i] = 4;
                            break;

                        case "empty":
                            orderValues [i] = 0;
                            break;
                        default:
                            orderValues [i] = 0;
                            break;
                    }
                }

                return orderValues;
            }

            /// <summary>
            /// Collect data on wrapping symbols
            /// </summary>
            /// <param name="_value">The value witht the symbols</param>
            /// <returns></returns>
            private static string [] Find_Wrapping ( string _value )
            {
                string [] wrappingValues = _value.Split ( ',' );

                if ( wrappingValues [0] == "empty" )
                {
                    wrappingValues [0] = string.Empty;
                }
                if ( wrappingValues [1] == "empty" )
                {
                    wrappingValues [1] = string.Empty;
                }

                return wrappingValues;
            }

            /// <summary>
            /// Collect block token
            /// </summary>
            /// <param name="_line">Line of code</param>
            /// <returns></returns>
            private static string [] Get_Token ( string _line )
            {

                string token = string.Empty;
                string value = string.Empty;
                string [] seperate = new string [2];

                if ( _line != string.Empty )
                {
                    if ( _line [0] == '<' )
                    {
                        if ( _line.Contains ( "=" ) )
                        {
                            token = _line.Split ( '<' ) [1].Split ( '>' ) [0].Split ( '=' ) [0];
                            value = _line.Split ( '<' ) [1].Split ( '=' ) [1].Split ( '>' ) [0];
                        }
                        else
                        {
                            token = _line.Split ( '<' ) [1].Split ( '>' ) [0];
                            value = string.Empty;
                        }
                    }

                }

                seperate [0] = token;
                seperate [1] = value;

                return seperate;
            }

            /// <summary>
            /// Details on the ingame block
            /// </summary>
            public struct BlockDetails
            {
                /// <summary>
                /// Type of the block
                /// </summary>
                public string Type { get; }
                /// <summary>
                /// Name of the block
                /// </summary>
                public string Name { get; }
                /// <summary>
                /// Info on the block
                /// </summary>
                public string Info { get; }
                /// <summary>
                /// which room the block is located in
                /// </summary>
                public string Room { get; }
                /// <summary>
                /// The name of the grid this block is placed on
                /// </summary>
                public string GridName { get; }

                private string Get_Value ( int _order )
                {
                    switch ( _order )
                    {
                        case 0:
                            return string.Empty;
                        case 1:
                            return Name;
                        case 2:
                            return Info;
                        case 3:
                            return Room;
                        case 4:
                            return GridName;
                        default:
                            break;
                    }

                    return null;
                }

                /// <summary>
                /// Output the end result
                /// </summary>
                /// <returns></returns>
                public string BlockString ()
                {
                    return $"{G_WrappingValues [G_Order [0] - 1] [0]}{Get_Value ( G_Order [0] )}{G_WrappingValues [G_Order [0] - 1] [1]}{G_WrappingValues [G_Order [1] - 1] [0]}{Get_Value ( G_Order [1] )}{G_WrappingValues [G_Order [1] - 1] [1]}{G_WrappingValues [G_Order [2] - 1] [0]}{Get_Value ( G_Order [2] )}{G_WrappingValues [G_Order [2] - 1] [1]}{G_WrappingValues [G_Order [3] - 1] [0]}{Get_Value ( G_Order [3] )}{G_WrappingValues [G_Order [3] - 1] [1]}";
                }

                /// <summary>
                /// Creates a new object of type BlockDetails
                /// </summary>
                /// <param name="_name">Name of the block</param>
                /// <param name="_info">Info on the block</param>
                /// <param name="_room">Room the block is located in</param>
                /// <param name="_gridName">The name of the grid this block is placed on</param>
                public BlockDetails ( string _type, string _name, string _info, string _room, string _gridName )
                {
                    Type = _type;
                    Name = _name;
                    Info = _info;
                    Room = _room;
                    GridName = _gridName;
                }

                public override string ToString ()
                {
                    return $"{Type}: {Name},{Info},{Room},{GridName}";
                }
            }
        }

        //to this comment.
        #region post-script
    }
}
#endregion