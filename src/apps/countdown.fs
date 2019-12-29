\ Countdown timer
\ (c)copyright 2019 by Gerald Wodni <gerald.wodni@gmail.com>

compiletoflash

45    variable countdown-timer
false variable countdown-active

: countdown-logo ( -- )
    7px-sans font !
    10000 frame-delay !
    clear
    1 cur-column !

    \ hourglass
    $000404 dup text-color ! color!
    d" x"
    roll-up
    1 0 goto
    5 0 line
    1 rows 1- goto
    5 rows 1- line

    \ sand
    $040400 color!
    2 1 goto 4 1 line
    3 4 point ;

\ select time in 5 minute steps
: countdown-select ( n-minutes -- )
    $070707 dup color! text-color !
    0 <# #S #> d-type ;

\ "countdown"
: countdown-count ( n-minutes -- )
    dup 5 < if
        5 0 goto
        $0F0000
    else
        dup 15 < if
            3 0 goto
            $070700
        else
            0 0 goto
            $000F00
        then
    then

    dup color! text-color !
    0 <# #S #> d-type 
    roll-up
    6 0 line ;

: countdown-run ( n-count n-count -- n-count2 )
    10000 frame-delay !
    5px-sans font !

    buttons-once@ case
        1 of
            countdown-active @ -1 xor countdown-active !
            2drop 0 0 \ reset running counter
        endof
        2 of -5 60 * countdown-timer +! endof
        4 of  5 60 * countdown-timer +! endof
    endcase

    clear

    countdown-active @ if
         \ frame
        100 = if
            drop 0 \ reset counter
            -1 countdown-timer +!
        then
        countdown-timer @
        60 /
        ?dup if
            countdown-count
        else
            countdown-timer @ 0= if
                d" 0!!" drop 0 \ frame-counter resetten
            else
                d" 0"
            then
        then
    else
        drop \ frame
        countdown-timer @ 60 /
        countdown-select
    then ;

' countdown-logo ' countdown-run 0 create-app
