\ XY Grid extensions for WS2812b
\ (c)copyright 2014, 2018 by Gerald Wodni <gerald.wodni@gmail.com>

compiletoflash

7 constant cols
7 constant rows
cols rows * constant leds
leds 4 * constant led-buffer-size
cols 4 * constant row-size
row-size 2/ constant row-size/2
led-buffer-size buffer: led-buffer

\ wave-like pattern
: buffer-wave
        led-buffer led-buffer-size bounds do
                i 2 rshift $F and i !
        4 +loop ;

: buffer! ( x-color -- )
	led-buffer led-buffer-size bounds do
		dup i !
	4 +loop drop ;

: buffer-off
	led-buffer led-buffer-size bounds do
		$0 i !
	4 +loop ;

: led-n ( n-index -- a-addr )
	4 * led-buffer + ;

' ! variable led-n!-xt

: led-n! ( x-color n-index -- )
	led-n led-n!-xt @ execute ;

: led-xy ( n-x n-y -- index )
	cols * + ;

: xy! ( x-color n-x n-y -- )
	over 0 cols between
	over 0 rows between
	and if			\ only draw in buffer region
		led-xy led-n!
	else
		2drop
	then ;

: xy@ ( n-x n-y -- x-color )
	led-xy led-n @ ;

: init-line 
	8 cols rows min min 0 do
		$0F0000 i cols * i + led-n!
	loop ;

\ linear flush in one sequence
: flush
	led-buffer led-buffer-size bounds do
		i @ >rgb
	4 +loop ; 

: init-pattern ( -- )
    buffer-wave
    init-line
    $001F00 0 0 xy! ;

: init-spi
    init-spi
    init-pattern
    flush ;

cornerstone wcold
