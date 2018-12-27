\ convert numerical ID to a name (stored in 32bit int)
\ (c)copyright 2018 by Gerald Wodni <gerald.wodni@gmail.com>

create vocals char a , char e , char i , char o , char u ,

0 variable idnamebuffer
: idname ( n -- )
    \ compute idname, store in idnamebuffer
    $B297FD74 xor       \ avoid 2 close numbers
    0 idnamebuffer !
    4 0 do
        dup $FF and     \ get last byte
        26 mod          \ convert to letter
        [char] a +
        idnamebuffer @  \ add as last letter
        8 lshift or
        idnamebuffer !
        8 rshift
    loop drop ;

: idname. ( -- )
    \ print id name
    idnamebuffer 4 type ;

: idtest ( -- )
    26 0 do
        cr
        i idname
        idname.
    loop ;

idtest
bye
