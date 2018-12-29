\ Common color functions
\ (c)copyright 2014, 2018 by Gerald Wodni <gerald.wodni@gmail.com>

compiletoflash

: decay-color ( -- )
    dup $FF0000 and if $010000 - then   \ subtract red   ( if any )
    dup $00FF00 and if $000100 - then   \ subtract green ( if any )
    dup $0000FF and if $000001 - then ; \ subtract blue  ( if any )

: decay-grid ( -- )
    rows 0 do
        cols 0 do
            i j xy@
            decay-color
            i j xy!
        loop
    loop ;
