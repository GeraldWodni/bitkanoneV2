\ MPU-6050 motion tracking driver
\ (c)copyright 2018 by Gerald Wodni <gerald.wodni@gmail.com>

compiletoflash

$68 constant mpu-addr   \ i2c address

$6B constant PWR_MGMT_1 \ main configuration registers
$19 constant SMPLRT_DIV

$3B constant MPU_ACCEL_XOUT \ sensor start register
$41 constant MPU_TEMP_H     
$43 constant MPU_GYRO_XOUT  

14 constant #sens-reg   \ all sensor data: 6 accel + 2 temp + 6 gyro
#sens-reg   buffer:      ACCEL_XOUT \ accelerometer X,Y,Z (16bit 2s-complement)
ACCEL_XOUT  6 + constant TEMP_H     \ temperature H+L
TEMP_H      2 + constant GYRO_XOUT  \ gyrometer X,Y,Z (16bit 2s-complement)

: mpu@f ( n-reg -- x-value f )
    mpu-addr i2c-read ;

: mpu@ ( n-reg -- x-value )
    mpu@f drop ;

: mpu!f ( x-value n-reg -- f )
    mpu-addr i2c-write ;

: mpu! ( x-value n-reg -- f )
    mpu!f drop ;

: mpu-read ( -- f )
    \ read sensor data
    ACCEL_XOUT #sens-reg MPU_ACCEL_XOUT mpu-addr i2c-read-n
    \ swap high and low bytes
    ACCEL_XOUT #sens-reg bounds do
        i c@        \ fetch bytes
        i 1+ c@
        i c!        \ store reversed
        i 1+ c!
    2 +loop
    ;

\ : mpu-temp@ ( -- )
\     TEMP_H h@ 100 * \ extend to centigrade
\     340 / 3653 + ;  \ scale and offset

: mpu-temp@ ( -- )
    TEMP_H h@
    340 / 36 + ;  \ scale and offset

\ fetch and print all mpu sensors
: mpu. ( -- )
    mpu-read .
    cr ." AX:" ACCEL_XOUT     h@ sign-h .
    cr ." AY:" ACCEL_XOUT 2+  h@ sign-h .
    cr ." AZ:" ACCEL_XOUT 4 + h@ sign-h .
    cr ." Tx:" mpu-temp@                .
    cr ." GX:" GYRO_XOUT      h@ sign-h .
    cr ." GY:" GYRO_XOUT  2+  h@ sign-h .
    cr ." GZ:" GYRO_XOUT  4 + h@ sign-h .
    ;

: mpu.. ( -- )
    begin
        100000 0 do
            i2c/2
        loop
        mpu.
    key? until ;

: init-mpu ( -- )
    init-i2c

    $80 PWR_MGMT_1 mpu! \ induce reset
    begin
        PWR_MGMT_1 mpu@ $40 =
    until
    
    \ Init sequence by Kris Winer from:
    \ https://github.com/kriswiner/MPU6050/blob/master/MPU6050BasicExample.ino#L474
    $01 PWR_MGMT_1 mpu! \ disable sleep, x-gyro PLL

    \ Set sample rate = gyroscope output rate/(1 + SMPLRT_DIV)
    $01 SMPLRT_DIV mpu! \ Use a 200 Hz sample rate 
    ;

init-mpu
