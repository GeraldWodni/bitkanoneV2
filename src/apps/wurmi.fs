\ Wurmi - snake clone for the bitkanoneV2
\ (c)copyright 2018 by Helmar Wodtke <helmwo@gmail.com>

compiletoflash

   6000 constant balance
$000000 constant bgcolor
$060006 constant wcolor

: (white)     $1000000 * $060006 + ;
: (gold)      $1000000 * $060600 + ;
: (green)     $1000000 * $000f00 + ;

1 variable first-run
0 variable worm-head
0 variable worm-end
0 variable score

\ ---- number counter from 0..99 to show score ----

$101000 constant numcolor

: show3x5                  \ n x y --
  16384 swap 5 bounds do   \ y-loop
    over 3 bounds do       \ x-loop
      2 pick over and if numcolor else bgcolor
      then i j xy! 2 /
    loop
  loop 2drop drop ;

create numbs
  31599 , \ 0
   4681 , \ 1
  31183 , \ 2
  29647 , \ 3
   5100 , \ 4
  29671 , \ 5
  31719 , \ 6
   4687 , \ 7
  31727 , \ 8
  29679 , \ 9

: count-up                  \ n --
  0 dup 1 show3x5           \ clear first shown digit
  1 + 0 do
    i dup 10 / ?dup if cells numbs + @ 0 1 show3x5 then
    10 mod cells numbs + @ 4 1 show3x5
    flush
    100 ms                  \ delay
  loop ;

\ -----------------------------------------------------------------------




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
: move-worm                                   \ --
              worm-head move-head
              worm-end  move-end ;

: food?                                       \ p -- 0|n
              move-worm' p@ $1000000 u/ ;
: feed-it                                     \ n --
              cols 1 - random-max
              rows 1 - random-max cols * +
              dup p@ $1000000 u/ 0 = if p! else 2drop then ;
: wurmi-new-world                             \ --
              bgcolor buffer!
              0 score !
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

: clear-any-bonus                             \ -- 
             rows 0 do
               cols 0 do
                 i j xy@ $1000000 u/ 5 > if
                   bgcolor i j xy!
                 then
               loop
             loop ;

: inc-row    \ v row --
             cols 0 do
               2dup i swap xy@ +
               over i swap xy!
             loop 2drop ;
: inc-col    \ v col --
             rows 0 do
               2dup i xy@ +
               over i xy!
             loop 2drop ;

0 variable indic-on
0 variable indic-shown

: indic          \ v indicator --
            case
              1 of cols 1 - inc-col endof
              2 of        0 inc-col endof
              3 of rows 1 - inc-row endof
              4 of        0 inc-row endof
              drop
            endcase ;

: wurmi-logo ( -- )
    3000 frame-delay !

    buffer-off
    4 1 do
        $040004 i 3 xy!
    loop
    $040004 1 2 xy!
    $000400 5 3 xy! ;

: wurmi-run ( n -- )
    mpu-read drop
    mpu-xy@
\ initialize random by user moves
    2dup xor seed @ xor seed !
    dup        balance negate < if 2drop 1
    else       balance        > if drop  2
      else dup balance negate < if drop  3
        else   balance        > if       4 else 0 then
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

    futter-count   1 < if 5 (green) feed-it then
    100 random-max 0 = if clear-any-bonus
                          6  (gold) feed-it then

    indic-shown @ ?dup if (white) worm-head @ p! then

    worm-head @ food? case
      0 of move-worm                      endof
      5 of worm-head move-head 1 score +! endof
      6 of move-worm
           worm-head @ worm-end @ <> if
             worm-end move-end
           then                1 score +! endof
      wcolor buffer! flush 300 ms
      bgcolor buffer!
      score @ 99 umin count-up 1000 ms
      1 first-run !
    endcase
    2drop
    ;

' wurmi-logo ' wurmi-run 0 create-app
