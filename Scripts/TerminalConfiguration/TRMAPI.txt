'format' should represent the format of the block renaming. (To indicate that the formatting is over close off the code block with '/'. Exmaple: <format>Code goes here</format>)
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

Exmaple:
                <format>
                    order="nameinfo,room,gridName"
                    nameWrap=*,*
                    infoWrap=<,>
                    roomWrap=-,empty
                    gridWrap=(
                 </format>
                <naming=container>
                    name=Container
                    infoTag=Iron
                    room=Storage
                    gridName=this
                </naming>
                <naming=battery>
                    name=Battery
                    info=100%
                    room=PowerBank
                    gridName=this
                </naming>


                Should output: *name* <info> - room (gridname)
                The container blocks in the terminal should now be called: *Container* <Iron> - storage (Earth Base)
                And
                The Battery blocks in the terminal should now be called: *Battery* <100%> - Power Bank (Earth Base)


Supported blocks:
	hangarDoor
	slidingDoor
	vent
	massBlock
	assembler
	battery
	beacon
	buttenPanel
	camera
	container
	cockpit
	collector
	controlPanel
	sorter
	cryopod
	decoy
	door
	gasGen
	h2Tank
	o2Tank
	gravGen
	gyro
	interiorLight
	jumpDrive
	gear
	medbay
	oreDetector
	program
	reactor
	refinery
	remote
	sensor
	connector