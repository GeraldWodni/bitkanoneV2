/* Handheld + Diffusor for Bitkanone V2 */
/* (c)copyright 2018 by Gerald Wodni <gerald.wodni@gmail.com> */

/* general settings */
Columns = 7;
PlateWidth = 98;

ScreenThickness = 0.4;

PillarHeight = 7;

LedHeight = 2;
LedDistance = PlateWidth/Columns;

DiffusorHeight = PillarHeight-LedHeight;
LedDiameter = 4;
DiffusorDiameter = 8;

DiffusorWall = 0.5;

/* Wall cutouts */
OuterWall = 1;
InterConnectHeight = 3;
InterConnectWidth = 13;
UsbWidth = 10;
UsbHeight = 4;
UsbOffset = LedDistance*2-UsbWidth/2;

/* buttons */
/* button = printed; switch = mechanical */
ButtonDiameter = 5;
ButtonTop = ScreenThickness+2;
ButtonSpacing = 0.2;
SwitchWidth = 6   + ButtonSpacing*2;
SwitchDepth = 3.5 + ButtonSpacing*2;
SwitchHeight = 4.5;
SwitchBase = 2;
ButtonTopDiameter = ButtonDiameter-ButtonSpacing*2;
ButtonWall = 1;


/* helpers */
E = 0.01;
$fn=64;

include <Screw.scad>;

PillarWall=1.2;
PillarDiameter = M3head + PillarWall*2;;



E2 = E/2;

module screwPillar(x,y) {
    inset = PillarHeight-LedHeight;
    translate([x,y]){
        difference() {
            cylinder(d=PillarDiameter, h=PillarHeight);
            
            translate([0,0,PillarHeight])
            mirror([0,0,1])
            m3(0, 0, PillarHeight+ScreenThickness+E, inset);
        }
    }
}

module diffusor(x,y) {
    translate([x*LedDistance, y*LedDistance, -E]) {
        difference() {
            cylinder(d2=LedDiameter+DiffusorWall*2, d1=DiffusorDiameter+DiffusorWall*2, h=DiffusorHeight);
            cylinder(d2=LedDiameter, d1=DiffusorDiameter, h=DiffusorHeight+E);
        }
    }
}

PillarOffset = 14;
module plateGuides() {
    union()
    translate([-PlateWidth/2, -PlateWidth/2]) {
        
        /* screw pillars */
        screwPillar(PillarOffset, PillarOffset);
        screwPillar(PlateWidth-PillarOffset, PillarOffset);
        screwPillar(PillarOffset*2, PlateWidth-PillarOffset);
        
        /* diffusor guidances */
        translate([LedDistance/2, LedDistance/2])
        for( y = [0:Columns-1] )
            for( x = [0:Columns-1] )
                diffusor(x, y);
        
        /* plate - main diffusor */
        translate([0, 0, -ScreenThickness])
        cube([PlateWidth, PlateWidth, ScreenThickness]);
    }
}

module buttonHole(x,y) {
    translate([x * LedDistance, y * LedDistance])
    cylinder(d=ButtonDiameter, h=ScreenThickness+E);
}

module screwInset(x, y) {
    translate([x, y])
    cylinder(d=M3head, h=ScreenThickness+E);
}

module walls() {
    difference() {
        cube([PlateWidth, PlateWidth, PillarHeight]);
        translate([OuterWall, OuterWall, -E2])
        cube([PlateWidth-OuterWall*2, PlateWidth-OuterWall*2, PillarHeight+E]);
        
        translate([-E2, PlateWidth/2-InterConnectWidth/2, PillarHeight-InterConnectHeight])
        cube([PlateWidth+E, InterConnectWidth, InterConnectHeight+E]);
        translate([PlateWidth/2-InterConnectWidth/2, -E2, PillarHeight-InterConnectHeight])
        cube([InterConnectWidth, PlateWidth+E, InterConnectHeight+E]);
        
        translate([-E2, UsbOffset, PillarHeight-UsbHeight])
        cube([OuterWall+E, UsbWidth, UsbHeight+E]);
    }
}

module top() {
    difference() {
        union() {
            plateGuides();
            translate([-PlateWidth/2, -PlateWidth/2])
            walls();
        }
        translate([-PlateWidth/2, -PlateWidth/2, -ScreenThickness-E2]) {
            screwInset(PillarOffset, PillarOffset);
            screwInset(PlateWidth-PillarOffset, PillarOffset);
            screwInset(PillarOffset*2, PlateWidth-PillarOffset);

            buttonHole( 1, 5 );
            buttonHole( 5, 5 );
            buttonHole( 6, 5 );
        }
    }
}

module button() {
    baseHeight = PillarHeight - SwitchBase;
    cutoutHeight = SwitchHeight-SwitchBase;

    difference() {
        union() {
            /* top */
            translate([0, 0, baseHeight])
            cylinder( d = ButtonTopDiameter, h = ButtonTop );

            /* base */
            baseWidth = SwitchWidth + ButtonWall*2;
            baseDepth = SwitchDepth + ButtonWall*2;
            translate([-baseWidth/2, -baseDepth/2])
            cube([baseWidth, baseDepth, baseHeight]);
        }

        translate([-SwitchWidth/2, -SwitchDepth/2, -E])
        cube([SwitchWidth, SwitchDepth, cutoutHeight+E]);
    }
}

// top();
button();