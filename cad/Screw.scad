/* Screw Library */
/* (c)copyright 2018 by Gerald Wodni <gerald.wodni@gmail.com> */

/** M3 **/
M3shaft = 4;
M3head = 6.5;
M3transition = 0.5;
M3Nut = 7;
M3NutHeight = 2.5;
M3NutCage = 15;

NutDiameter = 7;

E = 0.1;
E2 = E/2;

module m3NutHole(x, y, h) {
    translate([x, y]) {
        translate([0, 0, -h-E2])
        cylinder(d=M3shaft, h=h+E, $fn=32);
        
        translate([0, 0, -NutDepth])
        cylinder(d=NutDiameter, h=h+E, $fn=6);
    }
}

module m3Padding(x, y, h, padding) {
    translate([x,y])
    cylinder( d=M3shaft + padding*2, h = h );
}

module m3(x = 0,y = 0, depth = 10, head = 0, nut = 0) {
    translate([x,y]) {
        /* shaft */
        translate([0,0,-E])
        cylinder(d=M3shaft, h=depth+E);
        
        /* transition */
        translate([0, 0, depth-head-E-M3transition])
        cylinder(d1=M3shaft, d2=M3head, M3transition+E*2);
        
        /* head */
        translate([0, 0, depth-head])
            cylinder(d=M3head, h=head+E);
        
        if( nut > 0 )
            translate([0,0,-E2])
            cylinder( d=M3Nut, h = E+nut, $fn=6 );
    }
}