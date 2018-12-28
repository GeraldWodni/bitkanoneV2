\ Standard utilities
\ (c)copyright 2018 by Gerald Wodni <gerald.wodni@gmail.com>

compiletoflash
: .s ( -- )
    \ display stack contents
    [CHAR] < emit depth 0 <# #s #> type [CHAR] > emit space depth 0 ?do
        depth i - 1- pick .
    loop ;

: cell ( -- n-cell-size )
    1 cells ;

: hex. ( u -- )
    base @ >r hex u. r> base ! ;

: bounds ( c-addr n -- addr-end addr-start )
    over + swap ;

: between ( n-val n-min n-max-1 -- f )
	>r over <=
	swap r> < and ;

\ sign extend 16 to 32 bit
: sign-h ( u1 -- n1 )
    dup $8000 and if
        $FFFF0000 or
    then ;

: between ( n-val n-min n-max-1 -- f )
	>r over <=
	swap r> < and ;

: dump ( c-addr n -- )
    cr
    bounds do
        i hex. ."  "
        i c@ hex. cr
    loop ;


: free-ram
	compiletoram
	flashvar-here here - u. ;

: free-flash
	compiletoflash
	$10000 here - u. ;

\ <<< Taken from USB driver for STM32F103 by Jean-Claude Wippler
\   configured for Shenzhen LC Technology board with STM32F103C8T6.
\ -----------------------------------------------------------------------------
\   Flash tools
\ -----------------------------------------------------------------------------

: bit ( u -- u )  \ turn a bit position into a single-bit mask
  1 swap lshift  1-foldable ;

\ emulate c, which is not available in hardware on some chips.
\ copied from Mecrisp's common/charcomma.txt
0 variable c,collection

: c, ( c -- )  \ emulate c, with h,
  c,collection @ ?dup if $FF and swap 8 lshift or h,
                         0 c,collection !
                      else $100 or c,collection ! then ;

: calign ( -- )  \ must be called to flush after odd number of c, calls
  c,collection @ if 0 c, then ;


: flash-kb ( -- u )  \ return size of flash memory in KB
  $1FFFF7E0 h@ ;
: flash-pagesize ( addr - u )  \ return size of flash page at given address
  drop flash-kb 128 <= if 1024 else 2048 then ;

: cornerstone ( "name" -- )  \ define a flash memory cornerstone
  <builds begin here dup flash-pagesize 1- and while 0 h, repeat
  does>   begin dup  dup flash-pagesize 1- and while 2+   repeat  cr
  eraseflashfrom ;

\ -----------------------------------------------------------------------------
\  Clock setup and timing
\ -----------------------------------------------------------------------------

\ adjusted for STM32F103 @ 72 MHz (original STM32F100 by Igor de om1zz, 2015)

: 72MHz ( -- )  \ set the main clock to 72 MHz, keep baud rate at 115200
  $12 FLASH_ACR !                 \ two flash mem wait states
  16 bit RCC_CR bis!              \ set HSEON
  begin 17 bit RCC_CR bit@ until  \ wait for HSERDY
  1 16 lshift                     \ HSE clock is 8 MHz Xtal source for PLL
  7 18 lshift or                  \ PLL factor: 8 MHz * 9 = 72 MHz = HCLK
  4  8 lshift or                  \ PCLK1 = HCLK/2
  2 14 lshift or                  \ ADCPRE = PCLK2/6
            2 or  RCC_CFGR !      \ PLL is the system clock
  24 bit RCC_CR bis!              \ set PLLON
  begin 25 bit RCC_CR bit@ until  \ wait for PLLRDY
  625 USART1 USARTx_BRR !         \ fix console baud rate
;

\ -----------------------------------------------------------------------------
\ >>> Taken from USB driver for STM32F103 by Jean-Claude Wippler
\ -----------------------------------------------------------------------------

\ Further utils

\ Wait for any bit of mask in register to become true
: wait4true ( x-mask h-addr -- )
    begin
        2dup h@ and
    until 2drop ;

\ Wait for all bits of mask in register to become false
: wait4false ( n-bit h-addr -- )
    begin
        2dup h@ and 0=
    until 2drop ;

cornerstone cold
