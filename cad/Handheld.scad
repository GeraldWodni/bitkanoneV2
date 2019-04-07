/* Handheld + Diffusor for Bitkanone V2 */
/* (c)copyright 2018 by Gerald Wodni <gerald.wodni@gmail.com> */

/* general settings */
Columns = 7;
PlateWidth = 98;
LedDistance = PlateWidth/Columns;
Wall = 3;
Bottom = 2;

Height = 13+Bottom;

/* Wall cutouts */
InterConnectHeight = 3;
InterConnectWidth = 13;

/* helpers */
E = 0.01;
E2 = E/2;
$fn=64;


Cutouts = [
    /*x, y, w, h, wall */
    [26, PlateWidth-19, 47-26, 19-6, 0.8], /* X401 - Servos */
    [27, 12, 48-27, 22-12, 0.8], /* X42 */
    [77, 26, 86-77, 33-26, 0], /* X200-Serial */
    [15, 39, 21-15, 43-39, 0]  /* J202 5VUSB */
    
];

/* Pillars */
include <Screw.scad>;

PillarWall=2;
PillarDiameter = M3head + PillarWall*2;
NutBase = 2;

/* reset-button */
/* button = printed; switch = mechanical */
ButtonDiameter = 5;
ButtonTop = Bottom;
ButtonSpacing = 0.2;
ResetX = 63;
ResetY = 45.5;
SwitchWidth = 6   + ButtonSpacing*2;
SwitchDepth = 3.5 + ButtonSpacing*2;
SwitchHeight = 4.5;
SwitchBase = 2;
ButtonTopDiameter = ButtonDiameter-ButtonSpacing*2;
ButtonWall = 1;

/* modules */

module ledOffset( x, y ) {
    translate([x * LedDistance, y * LedDistance])
    children();
}

module pillar() {
    cylinder(d=PillarDiameter, h=Height);
}

module interconnect() {
    translate([-PlateWidth/2-E2, -InterConnectWidth/2])
    cube([PlateWidth+E, InterConnectWidth, InterConnectHeight+E]);
}

module base() {
    difference() {
        cube([PlateWidth, PlateWidth, Height]);
        translate([Wall, Wall, Bottom])
        cube([PlateWidth-Wall*2, PlateWidth-Wall*2, Height-Bottom+E]);
        
        translate([PlateWidth/2, PlateWidth/2, Height-InterConnectHeight]) {
            interconnect();
            
            rotate([0, 0, 90])
            interconnect();
        }
    }
    ledOffset( 1, 1 ) pillar();
    ledOffset( 6, 1 ) pillar();
    ledOffset( 5, 6 ) pillar();
    
    for( c = Cutouts ) {
        
        if( c[4] > 0 ) {
            translate([c[0]-c[4], c[1]-c[4]], -E2)
            cube([c[2]+c[4]*2, c[3]+c[4]*2, Height+E2/2]);
        }
    }
}

module pillarNut() {
    translate([0, 0, Height-NutBase])
    rotate([180,0,0])
    m3NutHole( 0, 0, Height );
}

module cutouts() {
    ledOffset( 1, 1 ) pillarNut();
    ledOffset( 6, 1 ) pillarNut();
    ledOffset( 5, 6 ) pillarNut();
    
    for( c = Cutouts ) {
        translate([c[0], c[1], -E2])
        cube([c[2], c[3], Height+E]);
    }
    
    translate([ResetX, ResetY, -E2])
    cylinder(d=ButtonDiameter, h=Bottom+E);
}

module body() {
    difference() {
        base();
        cutouts();
    }
}

body();