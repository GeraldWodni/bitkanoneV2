\ gforth mecrisp emulation

include test/ttester.fs

: compiletoflash cr ." compiletoflash" cr ;
: compiletoram   cr ." compiletoram"   cr ;

: variable create , does> ;

: xy! ( x-col x y -- )
    cr ." xy!: "
    swap . . hex. ;

