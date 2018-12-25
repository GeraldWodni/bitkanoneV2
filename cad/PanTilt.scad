/* Pan/Tilt Servo Platform for the Bitkanone V2 */
/* (c)copyright 2018 by Gerald Wodni <gerald.wodni@gmail.com> */

PlateWidth = 96;
PlateScrewOffset = 14;

PlateThickness = 2;
NutDepth = 0.5;

ScrewPadding = 2;
//$fn = 64;
$fn = 12;

include <Screw.scad>;

module m3Hole( x, y ) {
    translate([x,y,-PlateWidth/2])
    cylinder( d=M3shaft, h = PlateWidth );
}

module m3Plate( x, y ) {
    translate([x,y,0])
    cylinder( d=M3shaft + PlatePadding*2, h = PlateThickness );
}

module bitPlate() {
    //%cube([PlateWidth, PlateWidth, 0.1]);
    
    screwXYs = [
        // Top Left
        [PlateScrewOffset, PlateWidth-PlateScrewOffset],
        // Top Right
        [PlateWidth-PlateScrewOffset, PlateWidth-PlateScrewOffset],
        // Bottom
        [PlateWidth-PlateScrewOffset*2, PlateScrewOffset]
    ];
    
    // hulled, padded screws
    difference() {
        hull()
        for( i = [0:len(screwXYs)-1] )
            m3Padding(screwXYs[i][0], screwXYs[i][1], PlateThickness, ScrewPadding);
        
        for( i = [0:len(screwXYs)-1] )
            #m3(screwXYs[i][0], screwXYs[i][1], nut=NutDepth);
    }
}


//bitPlate();

module servoArmP( outerDiameter, centerDiameter, spacing, centerHeight, h ) {
    hull() {
        translate( [ spacing, 0, 0 ] )
        cylinder( d = outerDiameter, h = h );
        
        cylinder( d = centerDiameter, h = h );
        
        translate( [-spacing, 0, 0 ] )
        cylinder( d = outerDiameter, h = h );
    }
    cylinder( d = centerDiameter, h = centerHeight );
}

module servoArm() {
    OuterDiameter = 5;
    CenterDiameter = 7;
    CrossDistance = 10;
    CrossHeight = 10;
    CenterHeight = 30;
 
    servoArmP( OuterDiameter, CenterDiameter, CrossDistance, CenterHeight, CrossHeight );
}

module servoCross() {
    OuterDiameter = 5;
    CenterDiameter = 7;
    CrossDistance = 10;
    CrossHeight = 10;
    CenterHeight = 20;
 
    servoArmP( OuterDiameter, CenterDiameter, CrossDistance, CenterHeight, CrossHeight );
    rotate([0, 0, 90])
    servoArmP( OuterDiameter, CenterDiameter, CrossDistance, CenterHeight, CrossHeight );
}

servoCross();
translate([0, 20, 0])
servoArm();