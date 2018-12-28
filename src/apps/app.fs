\ App system for bitkanoneV2
\ (c)copyright 2018 by Gerald Wodni <gerald.wodni@gmail.com>

compiletoflash

\ app structue:
\ 0: xt-logo
\ 4: xt-run
\ 8: previous-app

create empty-app 0 , 0 ,  0 ,
empty-app variable last-app

\ create new app
: create-app ( xt-logo xt-run -- )
    cr ." CREATE:" 2dup swap hex. hex.
    here >r
    swap , , last-app @ , \ create new app and point to last one
    r> last-app ! ;    \ set current app as last

\ print app xt and return previous app
: app. ( addr -- addr-prev )
    cr
    dup @ hex.         \ show init
    dup cell+ @ hex.   \ show run
    2   cells + @ ;    \ fetch previous

\ list all apps' xts
: ls-apps ( -- )
    last-app @
    begin
        dup 0<>
    while
        app.
    repeat drop ;

\ get number of apps
: apps# ( -- n )
    0
    last-app @
    begin
        dup 0<>
    while
        swap 1+ swap
        2 cells + @
    repeat drop ;

\ get app per index
: app-n ( n -- addr )
    >r last-app @
    apps# r> over 1- min 0 max - 1- \ limit bounds
    0 ?do
        2 cells + @
    loop ;

: x ." LS:" apps# . cr ;

: xi ( -- ) ;
: xr ( -- ) ;

' xi ' xr create-app

: yi ( -- ) ;
: yr ( -- ) ;

' yi ' yr create-app

: xy. ( -- )
    cr ." xi:" ['] xi hex.
    cr ." xr:" ['] xr hex.
    cr ." yi:" ['] yi hex.
    cr ." yr:" ['] yr hex. ;
xy.
