\ Wurmi - snake clone for the bitkanoneV2
\ (c)copyright 2018 by Helmar Wodtke <helmwo@gmail.com>

compiletoflash

   6000 constant balance
$000000 constant bgcolor
$060006 constant wcolor

: (white)     $1000000 * $060006 + ;
: (green)     $1000000 * $000f00 + ;

1 variable first-run
0 variable worm-head
0 variable worm-end

: u/           u/mod nip ;
: p>xy         dup cols mod swap cols / ;
: p@           p>xy xy@ ;
: p!           p>xy xy! ;
: zcol?        dup cols mod 0 = ;             \ p -- p f
: move-worm'                                  \ p -- p'
              dup p@ $1000000 u/ case
                 1 of 1 + zcol? if cols - then             endof
                 2 of     zcol? if cols + then 1 -         endof
                 3 of cols + dup leds 1 - > if leds - then endof
                 4 of cols - dup        0 < if leds + then endof
              endcase ;
: move-head                                   \ p*
              dup @ dup  move-worm' swap p@ over p! swap ! ;
: move-end                                    \ p*
              dup @ tuck move-worm' swap ! bgcolor swap p! ;
: food?                                       \ p -- 0|n
              move-worm' p@ $1000000 u/ ;
: feed-it                                     \ --
              cols 1 - random-max
              rows 1 - random-max cols * +
              dup p@ $1000000 u/ 0 = if
                5 (green) swap p!
              else drop then ;
: wurmi-new-world                             \ --
              bgcolor buffer!
              cols 1 + 2 / dup rows * +
              dup worm-head ! worm-end !
              2 (white) worm-head @ p! ;
: futter-count                                \ -- n
             0
             rows 0 do
               cols 0 do
                 i j xy@ $1000000 u/ 5 = if
                   1 +
                   5 (green) i j xy!    \ refresh color
                 then
               loop
             loop ;

: inc-row    \ v row --
             cols 0 do
               2dup i swap xy@ +
               over i swap xy!
             loop 2drop ;
: inc-col    \ v row --
             rows 0 do
               2dup i xy@ +
               over i xy!
             loop 2drop ;

0 variable indic-on
0 variable indic-shown

: indic          \ v indicator --
            case
              2 of        0 inc-col endof
              1 of cols 1 - inc-col endof
              4 of        0 inc-row endof
              3 of rows 1 - inc-row endof
              drop
            endcase ;

: wurmi-logo ( -- )
    buffer-off clear
    $000400 text-color !
    d" w" ;

: wurmi-run ( n -- )
    3000 frame-delay !

    mpu-read drop
    mpu-xy@
    dup        balance        > if drop drop 2
    else       balance negate < if drop      1
      else dup balance        > if drop      4
        else   balance negate < if           3 else 0 then
        then
      then
    then

\ stack from now on: n indicator --

\ ---- small flash of light when changing direction
    -5 indic-on @ indic 0 indic-on !
    over 30 mod if
      dup if dup indic-shown @ <> if
        5 over dup indic-on ! dup indic-shown ! indic
      then then 2drop exit
    then
\ ----

    first-run @ if wurmi-new-world 0 first-run ! then

    futter-count 1 < if feed-it then

    indic-shown @ ?dup if (white) worm-head @ p! then

    worm-head @ food? ?dup if
      5 = if
        worm-head move-head
      else
        wcolor buffer!
        1 first-run !
      then
    else
      worm-head move-head
      worm-end  move-end
    then
    2drop
    ;

' wurmi-logo ' wurmi-run create-app
