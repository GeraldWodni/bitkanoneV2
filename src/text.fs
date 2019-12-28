\ High-Level Display interface: Fonts & Images
\ (c)copyright 2014, 2019 by Gerald Wodni <gerald.wodni@gmail.com>

compiletoflash

$000400 variable text-color
\ TODO: cvariable would be sufficient
cols variable max-column    \ stop printing at this column
  0 variable cur-column     \ current column
  0 variable col-offset     \ column offset
  0 variable row-shift      \ "cursive" scroll adaption
  1 variable boldness       \ times to repeat pattern
7px-sans variable font

0 variable cur-text

\ get address of symbol
: str-bounds ( c-addr -- c-addr-end c-addr-start ) dup c@ bounds 1+ swap 1+ swap ;

\ font-sizes, usage: "regular wide
: wide 3 boldness ! ;
: bold 2 boldness ! ;
: regular 1 boldness ! ;

\ get start-address of char
: c-pos ( u-char -- )
    font @ swap $7F and $20 - 0 ?do dup c@ 1+ + loop ;

: offset-column ( n-offset n-column )
    cur-column ! col-offset ! ;
: column ( n-column -- )
    0 swap offset-column ;

: >d ( x-char -- )
    cur-column @
    8 0 do
        over $01 and if \ bit lit?
            \ x-char n-col --
            text-color @ over

            i row-shift @ < if
                1+
            then

            rows i - 1- \ text is stored LSB bottom
            xy!
        then

        swap 1 rshift swap \ next bit
    loop 2drop ;

\ emit single byte and respect column-max
: d-emit-max ( x-char -- )
    cur-column @ max-column @ < if \ don't print after max-column
        >d
        1 cur-column +!
    else
        drop
    then ;

\ emit single byte and respect column-offset
: d-emit-off ( x-char -- )
    col-offset @ 0= if \ don't print offsets
        d-emit-max
    else
        col-offset @ dup 0< if  \ negative offset: shift ahead
            abs cur-column !
            0 col-offset !
            d-emit-max
        else    \ positive offset: skip column
            1- col-offset !
            drop
        then
    then ;

\ emit pattern n-times
: d-emit-n ( x-char n-count -- )
    boldness @ 0 do dup d-emit-off loop drop ;

: d-emit-byte ( x-char -- )
    d-emit-n ;

\ emit single char
: d-emit ( u-char -- )
    c-pos str-bounds do
        i c@ d-emit-byte
    loop
    $00 d-emit-byte ;


\ : d-emit dup ." EMIT:  " emit cr d-emit ;
: d-type ( c-addr n -- )
    \ str-bounds ?do i c@ d-emit loop ;
    bounds ?do i c@ d-emit loop ;

: d-length ( c-addr n -- n-len )
    \ 0 swap str-bounds
    0 -rot bounds
    ?do
        i c@ c-pos c@ +
        1+
    loop
    dup 0 > if
        1-
    then boldness @ * ;

: d( [char] ) parse d-type flush immediate ;

: d" postpone s" postpone d-type immediate ;

: dc-type ( c-addr n )
    2dup d-length 2/ cols 2/ swap - cur-column ! \ center cur-column
    d-type ;

\ centered varriants of d( and d"
: dc( [char] ) parse dc-type flush immediate ;
: dc" postpone s" postpone dc-type immediate ;

: clear
    buffer-off
    0 row-shift !
    0 cur-column ! ;

\ straight scroller
: |-scroller ( c-addr n -- )
    d-type flush
    100 ms ;

\ cursive scroller
: /-scroller ( c-addr n -- )
    0 7 do
        buffer-off
        i row-shift !

        col-offset @ cur-column @ 2>r
        2dup d-type flush

        2r> offset-column
        8 ms
    -1 +loop 2drop ;

' /-scroller variable scroller

: >scroll ( c-addr n -- )
    2dup d-length 1+ cols negate do
        i 0 offset-column
        clear
        2dup scroller @ execute
    loop 2drop ;

: scroll( [char] ) parse >scroll immediate ;

: test-text
    s" Hi there can anybody read this?" >scroll ;

: row-shift-test
    clear
    1 col-offset !
    4 row-shift !
    \ TODO: blue, previous ! should be visible
    $000001 text-color !
    d" !"
    $000100 text-color !
    d" a"
    $010000 text-color !
    d" !" flush ;

cornerstone tcold

\ init-mpu
\ row-shift-test
\ test-text
\ ' |-scroller scroller !
\ test-text
