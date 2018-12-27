/* Pan/Tilt Servo Platform for the Bitkanone V2 */
/* (c)copyright 2018 by Gerald Wodni <gerald.wodni@gmail.com> */

/* usage: scroll to bottom, uncomment part you want to render */

PlateWidth = 96;
PlateScrewOffset = 14;

PlateThickness = 2;
NutDepth = 0.5;

ScrewPadding = 2;
$fn = 64;
//$fn = 12;

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
    
    translate([-PlateWidth/2, -PlateWidth/2, -PlateThickness])
    // hulled, padded screws
    difference() {
        hull()
        for( i = [0:len(screwXYs)-1] )
            m3Padding(screwXYs[i][0], screwXYs[i][1], PlateThickness, ScrewPadding);
        
        for( i = [0:len(screwXYs)-1] )
            translate([0,0,PlateThickness])
            mirror([0,0,1])
            #m3(screwXYs[i][0], screwXYs[i][1], nut=NutDepth);
    }
}

module servoArmP( outerDiameter, centerDiameter, spacing, centerHeight, h ) {
    hull() {
        translate( [ spacing/2, 0, 0 ] )
        cylinder( d = outerDiameter, h = h );
        
        cylinder( d = centerDiameter, h = h );
        
        translate( [-spacing/2, 0, 0 ] )
        cylinder( d = outerDiameter, h = h );
    }
    translate( [0, 0, -centerHeight/2] )
    cylinder( d = centerDiameter, h = centerHeight+h );
}

SE = 0.1; // ServoExtra
module servoArm() {
    OuterDiameter = 4.8+SE;
    CenterDiameter = 6+SE;
    ArmWidth = 34.4+SE*2;
    CrossDistance = ArmWidth-OuterDiameter*2+SE*2;
    CrossHeight = 1.5+SE;
    CenterHeight = 30;
 
    servoArmP( OuterDiameter, CenterDiameter, CrossDistance, CenterHeight, CrossHeight );
}

ServoCrossWidth = 24+SE;
ServoCrossHeight = 1.5+0.5+SE;
ServoCrossCenter = 5.8+SE*8;
module servoCross() {
    OuterDiameter = 4.2+SE;
    CenterDiameter = ServoCrossCenter;
    CrossDistance = ServoCrossWidth-OuterDiameter+SE*2;
    CenterHeight = 20;
    
    translate([0,0,-E]) {
        servoArmP( OuterDiameter, CenterDiameter, CrossDistance, CenterHeight, ServoCrossHeight+E );
        rotate([0, 0, 90])
        servoArmP( OuterDiameter, CenterDiameter, CrossDistance, CenterHeight, ServoCrossHeight+E );
    }
}

ServoHeight = 12+SE*2;
ServoWidth = 23.2+SE*2;
ServoHoleDistance = 28.2;
module servoMount(sh = 20) {
    Height = ServoHeight;
    Width = ServoWidth;
    Depth = 20;
    
    Hole = 1.8;
    HoleDistance = ServoHoleDistance;
    
    translate([-Width/2, -Height/2, -Depth])
    cube([Width, Height, Depth+E]);
    
    translate([-HoleDistance/2,0,-sh]) {
        cylinder(d=Hole, h=sh+E);
        translate([HoleDistance,0,0])
        cylinder(d=Hole, h=sh+E);
    }
}

// servoCross();
// translate([0, 20, 0])
// servoArm();
//servoMount();

WuppleHeight = 0.7;
module wupple() {
    /* strange "Wupple" */
    WuppleInner = 5*2;
    WuppleOuter = 8*2;
    translate([0, 0, 0])
    difference() {
        cylinder( d=WuppleOuter, h = WuppleHeight+E2);
        translate([0, 0, -E])
        cylinder( d=WuppleInner, h = WuppleHeight+E*2);
    }
}

MiddleWidth = 25;
Wall = 3;
Thickness = ServoCrossHeight*2;
module middle() {
    Depth=33;
    Width=MiddleWidth;
    ServoMargin = 1;
    Height = ServoMargin + ServoHeight/2+Wall;
    
    
    /* wall */
    difference() {
        translate([0, -Depth/2, -E])
        cube([Wall, Depth, Height+E]);
        
        translate([0, 0, ServoHeight/2+ServoMargin])
        rotate([0, -90, 0])
        rotate([0, 0, 90])
        servoMount();
    }
    
    /* base */
    rotate([180,0,0])
    translate([Width/2,0,0])
    difference() {
        translate([-Width/2, -Depth/2]) {
            cube([Width, Depth, Thickness]);
        }
        rotate([0, 0, 45])
        servoCross();
        
        translate([0, 0, Thickness-WuppleHeight])
        #wupple();
    }
}

PCBBackHeight = 18;
module top() {
    PCBThickness = 1.6;
    LedHeight = 1.6;
    Distance = PlateWidth/2 - PCBBackHeight - PCBThickness - LedHeight;
    Width = ServoCrossWidth;
    Height = Distance + ServoCrossWidth/2;
    
    HelperWidth = 13;
    RS232Depth = 5;
    RS232Width = 21;
    RS232YOffset = 20.5;
    
    
    translate([Thickness+MiddleWidth/2, 0, 0]) {
        // Helper
        translate([0, Wall/2])
        rotate([90,0,0])
        linear_extrude(Wall)
        polygon([
            [0, 0], 
            [0, Distance-ServoCrossCenter],
            [HelperWidth, 0]
        ]);
        
        // CrossMount    
        rotate([0, 0, 90])
        translate([0, 0, Distance])
        rotate([-90,0,0])
        difference() {
            translate([-Width/2, -Width/2])
            cube([Width, Height, Thickness]);
            rotate([0, 0, 45])
            #servoCross();
            
            translate([0, 0, Thickness-WuppleHeight])
            #wupple();
        }
    }
        
    difference() {
        bitPlate();
        translate([PlateWidth/2 - RS232Width, -RS232Depth/2+RS232YOffset, -PlateThickness-E2])
        #cube([RS232Width, RS232Depth, PlateThickness+E]);
    }
}

MountHeight = PlateWidth/2+10;
BottomWidth = 28.5;
ServoHoleDiff = (ServoHoleDistance - ServoWidth) / 2;
ServoWall = ServoHoleDiff * 2;
ServoMountOffset = 16.8+3;

module bottomWall() {
    hull() {
        translate([-BottomWidth/2, -ServoWall/2])
        cube([BottomWidth, ServoWall, E]);
    
        translate([-ServoHeight/2, -ServoWall/2, MountHeight-Wall])
        cube([ServoHeight, ServoWall, E]);
    }
}

module bottom() {
    Width = BottomWidth;
    Depth = ServoWidth + ServoWall * 2;
    ServoScrewDepth = 9;
    
    CableWidth = 9;
    CableHeight = 4;
    
    ZipperWidth = 5;
    ZipperHeight = 2;
    ZipperOffset = 9;
    
    translate([-Width/2, -Depth/2, -Wall])
    cube( [Width, Depth, Wall] );
    
    difference() {
        /* SideWalls */
        union() {
            WallDistance = Depth/2-ServoWall/2;
            translate([0,-WallDistance,0])
            bottomWall();
            translate([0, WallDistance,0])
            bottomWall();
        }
        
        #translate([0, 0, MountHeight-Wall-ServoMountOffset]) {
            /* servoScrews */
            translate([0, 0, ServoMountOffset+E])
            rotate([0, 0, 90])
            servoMount(ServoScrewDepth);
            
            /* Cable */
            //translate([-CableWidth/2, Depth/2-ServoWall-E2])
            //cube([CableWidth, ServoWall+E, CableHeight]);
        }
        
        translate([-ZipperWidth/2-ZipperOffset, -Depth/2-E2, E2])
        #cube([ZipperWidth, Depth+E, ZipperHeight]);
        
        translate([-ZipperWidth/2+ZipperOffset, -Depth/2-E2, E2])
        #cube([ZipperWidth, Depth+E, ZipperHeight]);
    }
}

SpacerWall = 1;

module spacer() {
    dOut = M3shaft+SpacerWall*2;
    dIn = M3shaft;
    h = PCBBackHeight;
    
    difference() {
        cylinder( d=dOut, h=h );
        translate([0, 0, -E2])
        cylinder( d=dIn, h=h+E);
    }
}

/* Just Uncomment what you want */
//spacer();
//bottom();
//middle();
top();