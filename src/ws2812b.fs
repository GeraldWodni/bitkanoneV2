\ WS2812 Driver for lm4f120-MECRISP
\ (c)copyright 2014,2018 by Gerald Wodni <gerald.wodni@gmail.com>

compiletoflash

: >ws ( x -- )
	8 0 do
		$8 			\ push "0" ($8) on stack
		over $80 and 0<> 	\ check msb
		$E and or 		\ msb=1, push "1" ($E) on stack
		spi!
		1 lshift
	loop drop ;

: >rgb ( -- )
	dup 8  rshift >ws 	\ green
	dup 16 rshift >ws 	\ red
	>ws ; 			\ blue

: n-leds ( x-color n-leds -- )
	0 do
		dup >rgb
	loop drop ;

49 constant leds

: on      $FFFFFF leds n-leds ;
: off     $000000 leds n-leds ;
: red	  $1F0000 leds n-leds ;
: yellow  $1F1F00 leds n-leds ;
: green   $001F00 leds n-leds ;
: cyan	  $001F1F leds n-leds ;
: blue	  $00001F leds n-leds ;
: magenta $1F001F leds n-leds ;
: white	  $1F1F1F leds n-leds ;

: bill leds 0 do
		$1F0000 >rgb
		$1F1F00 >rgb
		$001F00 >rgb
		$00001F >rgb
		$1F1F1F >rgb
	loop ;

init-spi
bill
