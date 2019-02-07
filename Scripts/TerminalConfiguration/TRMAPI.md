# The Language
The TRM language is a language used inside the Programmable Block's custom data.
It's used to customize the display-name of different terminal blocks.
With this language the user can define name, info, room and grid name.
The user can also define what value to display and in which order to display them.
Example:
```
<format>
    order=name,info,room,gridName
    nameWrap=empty,empty
    infoWrap=<,>
    roomWrap=-,empty
    gridWrap=(,)
</format>
<naming=container>
    name= Cargo Container
    info=Iron
    room=Storage
    gridname=MyGrid
</naming>

//Displayed name in terminal window:
//Cargo Container<Iron>-Storage(MyGrid)
```
Even though you see the comments '//' in the example, the code does not yet support comments or empty lines.
Keep that in mind.

When you've written your code inside the **programmable block's custom data**, you have to *compile* the script in order for it to interprete the changes inside the **custom data**. (Press "Compile") - you have to do this **every time** you've made changes.
To rename the terminal blocks press "Run" - this must be done **every time** you want to rename the blocks on the grid. 

### Syntax
The language is build upon HTML but with a few tweaks. (_It's a fairly simpel syntax and therefore easy to learn_)
Like in HTML we open a block of code with ```<>``` and close  it with ```</>```.
Each statement **has** to be on it's own line, and while you can have spaces on both sides of the equal sign (_=_)
the spaces on the right side of the sign, will be interpreted as part of the value.
This example:
```
name=Cargo Container
```
will output "_Cargo Container_" as the name in the terminal window.
While this example:
```
name=  CargoContainer
```
will output "   _CargoContainer_"as the name in the temrinal window.
**Notice** how the second example has a **space** in front of the name.

In some cases you can't have spaces after the equal sign (_=_).
Here is ```order``` a good example.
The reason is that ```order``` is a keyword that evaluates 4 keywords, which all of must be included in the statement.
```name```,```info```,```room``` and ```gridName```.
These 4 keywords can be organized as pleased and will represnt the order, in which the value of them will be displayed in the terminal window.
Example:
```
<format>
	order=name,info,room,gridName
</format>
``` 
As you can see the block is opened with ```<format>``` and closed with ```</format>```. Notice the slash (_'/'_).
This indicates that everything inside this block of code will be statements that can be stored as values.
We use a comma (_,_) to seperate values.
Example:
```
nameWrap=*,/
```
This will output "_*Cargo Container/_". The comma (_,_) seperates the value into two values.
```namwWrap``` is the keyword used to identify the wrapping of the value of ```name```.
Like with all wrapping keywords, the right side of the comma (_,_) is used in front of them value, while the left side of the comma (_,_) is used at the end of the value. As you can see above, the star (_*_) is printed before the _name_, and the slash (_/_) is placed at the end of the _name_.

# Tokens & Keywords
### Tokens
 - <**format**> & <**/format**>
   - Inside this block you can customize the formatting of the en result.
   - The block must contain all of the following statements: ```order```, ```nameWrap```, ```infoWrap```, ```roomWrap``` & ```gridWrap```.
 - <**naming=value**> & <**/naming**>
   - Inside this block you can customize the naming of a given block type.
   - The block must contain all of the following statments: ```name```, ```info```, ```room``` & ```gridName```.
   - The _value_ of the naming block, will be the object the code block will modifiy in the terminal window. Example: ```<naming=container>```.
### Keywords
 - ```order```      | **Can only be placed inside the ```<format>``` block.** Example: ```order=name,info,room,gridName```.
   - ```name```     | Placeholder for the value of _name_ inside the ```<naming>``` block
     - This also used within the ```<naming>``` block. Example: ```name=Some Name```.
   - ```info```     | Placeholder for the value of _info_ inside the _<naming>_ block
     - This also used within the ```<naming>``` block. Example: ```info=Some Info```.
   - ```room```     | Placeholder for the value of _room_ inside the _<naming>_ block
     - This also used within the ```<naming>``` block. Example: ```room=Some Room```.
   - ```gridName``` | Placeholder for the value of _gridName_ inside the _<naming>_ block
     - This also used within the ```<naming>``` block. Example: ```gridName=Some Name```.
     - This can also use the ```this```keyword, which refers to the grid the PB is placed on. Example: ```gridName=this```
 - ```nameWrap```   | Which symbols should be used to wrap the value of _name_ in. **Can only be placed in ```<format>``` block.**
   - Can use ```empty``` to leave the field empty. Example: ```nameWrap=empty,empty```.
 - ```infoWrap```   | Which symbols should be used to wrap the value of _info_ in. **Can only be placed in ```<format>``` block.**
   - Can use ```empty``` to leave the field empty. Example: ```infoWrap=empty,empty```.
 - ```roomWrap```   | Which symbols should be used to wrap the value of _room_ in. **Can only be placed in ```<format>``` block.**
   - Can use ```empty``` to leave the field empty. Example: ```roomWrap=empty,empty```.
 - ```gridWrap```   | Which symbols should be used to wrap the value of _gridName_ in. **Can only be placed in ```<format>``` block.**
   - Can use ```empty``` to leave the field empty. Example: ```gridWrap=empty,empty```.
 - ```this```       | Refers to the grid the programmable object is placed on.
 - ```show```       | Wether or not the object should be shown in the terminal window. **Can only be placed in ```<naming>``` block.**
   - Can be either _false_ (Hidden) or _true_ (Visible).  Example: ```show=false```.
### Supported blocks
- ```hangarDoor```
   - Used to acces the _Hangar Doors_ on a grid. Example: ```<naming=hangarDoors>```
- ```slidingDoor```
   - Used to acces the _Sliding Doors_ on a grid. Example: ```<naming=slidingDoors>```
- ```vent```
   - Used to acces the _Vents_ on a grid. Example: ```<naming=vent>```
- ```mass```
   - Used to acces the _Mass Blocks_ on a grid. Example: ```<naming=mass>```
- ```assembler```
   - Used to acces the _Assemblers_ on a grid. Example: ```<naming=assembler>```
- ```battery```
   - Used to acces the _Batteries_ on a grid. Example: ```<naming=battery>```
- ```beacon```
   - Used to acces the _Beacons_ on a grid. Example: ```<naming=beacon>```
- ```buttonPanel```
   - Used to acces the _Button Panels_ on a grid. Example: ```<naming=buttonPanel>```
- ```camera```
   - Used to acces the _Cameras_ on a grid. Example: ```<naming=camera>```
- ```container```
   - Used to acces the _Cargo Containers_ on a grid. Example: ```<naming=container>```
- ```cockpit```
   - Used to acces the _Cockpits_ on a grid. Example: ```<naming=cockpits>```
- ```collector```
   - Used to acces the _Collectors_ on a grid. Example: ```<naming=collector>```
- ```controlPanel```
   - Used to acces the _Control Panels_ on a grid. Example: ```<naming=controlPanel>```
- ```sorter```
   - Used to acces the _Sorters_ on a grid. Example: ```<naming=sorter>```
- ```cryopod```
   - Used to acces the _Cryo Chambers_ on a grid. Example: ```<naming=cryopod>```
- ```decoy```
   - Used to acces the _Decoys_ on a grid. Example: ```<naming=decoy>```
- ```door```
   - Used to acces the _Interior Doors_ on a grid. Example: ```<naming=door>```
- ```gasGen```
   - Used to acces the _H2O2 Generators_ on a grid. Example: ```<naming=gasGen>```
- ```h2Tank```
   - Used to acces the _Hydrogen Tanks_ on a grid. Example: ```<naming=h2Tank>```
- ```o2Tank```
   - Used to acces the _Oxygen Tanks_ on a grid. Example: ```<naming=o2Tank>```
- ```gravGen```
   - Used to acces the _Gravity Generators_ on a grid. Example: ```<naming=gravGen>```
- ```gyro```
   - Used to acces the _Gyroscopes_ on a grid. Example: ```<naming=gyro>```
- ```jumpDrive```
   - Used to acces the _Jump Drives_ on a grid. Example: ```<naming=jumpDrive>```
- ```gear```
   - Used to acces the _Landing Gears_ on a grid. Example: ```<naming=gear>```
- ```light```
   - Used to acces the _Lights_ on a grid. Example: ```<naming=light>```
- ```medbay```
   - Used to acces the _Medical Room_ on a grid. Example: ```<naming=medbay>```
- ```advancedRotor```
   - Used to acces the _Advanced Rotors_ on a grid. Example: ```<naming=advancedRotot>```
- ```rotor```
   - Used to acces the _Rotors_ on a grid. Example: ```<naming=rotor>```
- ```wheel```
   - Used to acces the _Suspensions_ on a grid. Example: ```<naming=wheels>```
- ```oreDetector```
   - Used to acces the _Ore Detectors_ on a grid. Example: ```<naming=oreDector>```
- ```o2Farm```
   - Used to acces the _Oxygen Farms_ on a grid. Example: ```<naming=o2Farm>```
- ```parachute```
   - Used to acces the _Parachutes_ on a grid. Example: ```<naming=parachute>```
- ```piston```
   - Used to acces the _Pistons_ on a grid. Example: ```<naming=piston>```
- ```projector```
   - Used to acces the _Projector_ on a grid. Example: ```<naming=projector>```
- ```antenna```
   - Used to acces the _Antennas_ on a grid. Example: ```<naming=antenna>```
- ```reactor```
   - Used to acces the _Reactors_ on a grid. Example: ```<naming=reactor>```
- ```refinery```
   - Used to acces the _Refineries_ on a grid. Example: ```<naming=refinery>```
- ```remote```
   - Used to acces the _Remote Controls_ on a grid. Example: ```<naming=remote>```
- ```sensor```
   - Used to acces the _Sensors_ on a grid. Example: ```<naming=sensor>```
- ```connector```
   - Used to acces the _Connectors_ on a grid. Example: ```<naming=connector>```
- ```drill```
   - Used to acces the _Drills_ on a grid. Example: ```<naming=drill>```
- ```grinder```
   - Used to acces the _Grinders_ on a grid. Example: ```<naming=grinder>```
- ```merge```
   - Used to acces the _Merge Blocks_ on a grid. Example: ```<naming=merge>```
- ```welder```
   - Used to acces the _Welders_ on a grid. Example: ```<naming=welder>```
- ```solar```
   - Used to acces the _Solar Panels_ on a grid. Example: ```<naming=solar>```
- ```lcd```
   - Used to acces the _LCD Panels_ on a grid. Example: ```<naming=lcd>```
- ```timer```
   - Used to acces the _Timers_ on a grid. Example: ```<naming=timer>```
