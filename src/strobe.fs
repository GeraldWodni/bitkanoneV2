\ Strobe functions
\ (c)copyright 2018 by Gerald Wodni <gerald.wodni@gmail.com>

: blitz on 60 us off ;
: strobe ( n-ms -- ) begin blitz dup ms key? until drop ;
