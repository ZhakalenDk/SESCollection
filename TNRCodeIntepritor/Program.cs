using System;
using System.Collections;
using System.Collections.Generic;

namespace TNRCodeIntepritor
{
    class Program
    {
        static void Main ( string [] args )
        {
            string code1 = "<format=Earth Base>\norder=name,info,room,gridName\nnameWrap=*,*\ninfoWrap= <,> \nroomWrap=- , \ngridWrap=(,)\n</format>\n";
            string code2 = "<naming=Battery>\nname=Battery\ninfo=100%\nroom=Power Bank\ngridName=this\n</naming>\n";
            string code3 = "<naming=Container>\nname=Cargo Container\ninfo=Iron\nroom=Storage\ngridName=this\n</naming>\n";

            string code = code1 + code2 + code3;

            Console.WriteLine ( code );

            Interpreter.Interprete ( code );

            Console.WriteLine ( $"Block1 {Interpreter.Blocks [0].ToString ()}\nBlock2 {Interpreter.Blocks [1].ToString ()}" );
            Console.WriteLine ( $"End Result: {Interpreter.Blocks [0].BlockString()}" );

            Console.ReadKey ();
        }
    }


    static class Interpreter
    {
        public static List<BlockDetails> Blocks = new List<BlockDetails> ();
        private static string G_Name;
        private static string G_Info;
        private static string G_Room;
        private static string G_GridName;

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
                    Console.WriteLine ( $"Found start of block: {_lines [i]}" );

                    tokenSubValue = Get_Token ( _lines [i] ) [1];

                    for ( int j = i; j < _lines.Length; j++, i++ )
                    {
                        if ( Get_Token ( _lines [j] ) [0] == "/format" || Get_Token ( _lines [j] ) [0] == "/naming" )
                        {
                            if ( Get_Token ( _lines [j] ) [0] == "/naming" )
                            {
                                BlockDetails block = new BlockDetails ( tokenSubValue, G_Name, G_Info, G_Room, G_GridName );
                                Console.WriteLine ( $"Storing Block data: {block.ToString ()}" );
                                Blocks.Add ( block );
                            }

                            Console.WriteLine ( $"Found end of block: {_lines [j]}" );
                            break;
                        }

                        //Console.WriteLine ( $"Found statement: {_lines [j]}" );

                        if ( _lines [j].Contains ( "=" ) )
                        {
                            Execute ( _lines [j] );
                        }

                    }
                }
            }
            //Console.WriteLine ( $"{this.wrappingValues [this.order [0] - 1] [0]}{this.orderValues [this.order [0]]}{this.wrappingValues [this.order [0] - 1] [1]}{this.wrappingValues [this.order [1] - 1] [0]}{this.orderValues [this.order [1]]}{this.wrappingValues [this.order [1] - 1] [1]}{this.wrappingValues [this.order [2] - 1] [0]}{this.orderValues [this.order [2]]}{this.wrappingValues [this.order [2] - 1] [1]}{this.wrappingValues [this.order [3] - 1] [0]}{this.orderValues [this.order [3]]}{this.wrappingValues [this.order [3] - 1] [1]}" );
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
                    //Console.WriteLine ( $"Name Wrapping at [0] = {this.wrappingValues [0] [0]}{this.wrappingValues [0] [1]}" );
                    break;
                case "infoWrap":
                    G_WrappingValues [1] = Find_Wrapping ( value );
                    //Console.WriteLine ( $"Info Wrapping at [1] = {this.wrappingValues [1] [0]}{this.wrappingValues [1] [1]}" );
                    break;
                case "roomWrap":
                    G_WrappingValues [2] = Find_Wrapping ( value );
                    //Console.WriteLine ( $"Room Wrapping at [2] = {this.wrappingValues [2] [0]}{this.wrappingValues [2] [1]}" );
                    break;
                case "gridWrap":
                    G_WrappingValues [3] = Find_Wrapping ( value );
                    //Console.WriteLine ( $"Grid Name Wrapping at [3] = {this.wrappingValues [3] [0]}{this.wrappingValues [3] [1]}" );
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
                return "Earth Base"; // Change to Me.GridName
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
            string [] execute = _value.Split ( "," );   //  Split each keyword into an array
            for ( int i = 0; i < orderValues.Length; i++ )
            {
                string check = execute [i];
                //Console.WriteLine ( $"Execute: {check}" );
                switch ( check )
                {
                    case "name":
                        //Console.WriteLine ( $"Name at [{i}]" );
                        orderValues [i] = 1;
                        break;
                    case "info":
                        //Console.WriteLine ( $"Info at [{i}]" );
                        orderValues [i] = 2;
                        break;
                    case "room":
                        //Console.WriteLine ( $"Room at [{i}]" );
                        orderValues [i] = 3;
                        break;
                    case "gridName":
                        //Console.WriteLine ( $"Grid Name at [{i}]" );
                        orderValues [i] = 4;
                        break;

                    case "empty":
                        //Console.WriteLine ( $"Empty at [{i}]" );
                        orderValues [i] = 0;
                        break;
                    default:
                        //Console.WriteLine ( $"Empty at [{i}]" );
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
            string [] wrappingValues = _value.Split ( "," );

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

            private string Get_Value (int _order)
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
                return $"{G_WrappingValues [G_Order [0] - 1][0]}{Get_Value (G_Order[0])}{G_WrappingValues [G_Order [0] - 1] [1]}{G_WrappingValues [G_Order [1] - 1] [0]}{Get_Value ( G_Order [1] )}{G_WrappingValues [G_Order [1] - 1] [1]}{G_WrappingValues [G_Order [2] - 1] [0]}{Get_Value ( G_Order [2] )}{G_WrappingValues [G_Order [2] - 1] [1]}{G_WrappingValues [G_Order [3] - 1] [0]}{Get_Value ( G_Order [3] )}{G_WrappingValues [G_Order [3] - 1] [1]}";
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
}
