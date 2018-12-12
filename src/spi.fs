\ SPI driver for STM32F103
\ (c)copyright 2018 by Gerald Wodni <gerald.wodni@gmail.com>

: spi-wait-ready ( -- )
    0
    begin
        1+
        SPI2 SPIx_SR $82 and 0=
    until . ;

: spi! ( x -- )
    spi-wait-ready
    SPI2 SPIx_DR h! ;

: spi. ( -- )
    \ print spi status
    SPI2 SPIx_SR h@ hex.
    SPI2 SPIx_DR h@ hex. ;

: init-spi
    72mhz
    
    $4000 RCC_APB1ENR bis!  \ enable SPI2 Clock

    \ set pin to SPI
    ws-din gpio-alternate

    \ Master Mode: Set SSM and SSI in CR1

    \     DFF Data frame format 0:8bit, 1:16bit
    \     | SSM: Software slave management
    \     | |SSI: Internal slave select
    \     | ||LSB First
    \     | |||SPIE SPI Enable
    \     | ||||BR Baud rate control
    \     | |||||  Master selection
    \     | |||||  |CPOL
    \     | |||||\ ||Cphase
    \ 111111|||||\\|||
    \ 5432109876543210
     %0000001101111100 SPI2 SPIx_CR1 h!

    \ TXEIE: TX Buffer empty interrupt enable
    \ |RXNEIE: RX Buffer not empty interrupt enable
    \ ||ERRIE: Error interrupt enable
    \ |||Reserved
    \ |||| SSOE: SS output enable
    \ |||\\|TXDMAEN: TX buffer DMA enable
    \ |||\\||RXDMAEN: RX buffer DMA enable
    \ |||\\|||
    \ 76543210
     %00000100 SPI2 SPIx_CR2 h!
     ;
init-spi

: mspi 16 spi! 16 spi! 16 spi! 16 spi! ;
mspi
