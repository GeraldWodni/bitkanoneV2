\ adopted from Mecrisp by Matthias Koch
compiletoflash

\ add offset to address
: offset ( C: u1 --  R: u2 -- u3 )
    <builds ,
    does> @ + ;

\ === Flexible static memory controller ===
$A0000000 constant FSMC  
FSMC $0 + constant FSMC_BCR1
FSMC $4 + constant FSMC_BTR1
FSMC $8 + constant FSMC_BCR2
FSMC $C + constant FSMC_BTR2
FSMC $10 + constant FSMC_BCR3
FSMC $14 + constant FSMC_BTR3
FSMC $18 + constant FSMC_BCR4
FSMC $1C + constant FSMC_BTR4
FSMC $60 + constant FSMC_PCR2
FSMC $64 + constant FSMC_SR2
FSMC $68 + constant FSMC_PMEM2
FSMC $6C + constant FSMC_PATT2
FSMC $74 + constant FSMC_ECCR2
FSMC $80 + constant FSMC_PCR3
FSMC $84 + constant FSMC_SR3
FSMC $88 + constant FSMC_PMEM3
FSMC $8C + constant FSMC_PATT3
FSMC $94 + constant FSMC_ECCR3
FSMC $A0 + constant FSMC_PCR4
FSMC $A4 + constant FSMC_SR4
FSMC $A8 + constant FSMC_PMEM4
FSMC $AC + constant FSMC_PATT4
FSMC $B0 + constant FSMC_PIO4
FSMC $104 + constant FSMC_BWTR1
FSMC $10C + constant FSMC_BWTR2
FSMC $114 + constant FSMC_BWTR3
FSMC $11C + constant FSMC_BWTR4
        
	
\ === Power control ===
$40007000 constant PWR  
PWR $0 + constant PWR_CR
PWR $4 + constant PWR_CSR
        
	
\ === Reset and Clock Control ===
$40021000 constant RCC  
RCC $0 + constant RCC_CR
RCC $4 + constant RCC_CFGR
RCC $8 + constant RCC_CIR
RCC $C + constant RCC_APB2RSTR
RCC $10 + constant RCC_APB1RSTR
RCC $14 + constant RCC_AHBENR
RCC $18 + constant RCC_APB2ENR
RCC $1C + constant RCC_APB1ENR
RCC $20 + constant RCC_BDCR
RCC $24 + constant RCC_CSR
        
	
\ === GPIO ===
$40010800 constant GPIOA  
$40010C00 constant GPIOB  
$40011000 constant GPIOC  
$40011400 constant GPIOD  
$40011800 constant GPIOE  
$40011C00 constant GPIOF  
$40012000 constant GPIOG  

$0  offset GPIOx_CRL
$4  offset GPIOx_CRH
$8  offset GPIOx_IDR
$C  offset GPIOx_ODR
$10 offset GPIOx_BSRR
$14 offset GPIOx_BRR
$18 offset GPIOx_LCKR


\ === Alternate function IO ===
$40010000 constant AFIO  

AFIO $0 + constant AFIO_EVCR
AFIO $4 + constant AFIO_MAPR
AFIO $8 + constant AFIO_EXTICR1
AFIO $C + constant AFIO_EXTICR2
AFIO $10 + constant AFIO_EXTICR3
AFIO $14 + constant AFIO_EXTICR4
AFIO $1C + constant AFIO_MAPR2

\ === External interrupt/event controller ===
$40010400 constant EXTI  
EXTI $0 + constant EXTI_IMR
EXTI $4 + constant EXTI_EMR
EXTI $8 + constant EXTI_RTSR
EXTI $C + constant EXTI_FTSR
EXTI $10 + constant EXTI_SWIER
EXTI $14 + constant EXTI_PR

\ === Direct Memory Access Controller ===
$40020000 constant DMA1  
$40020400 constant DMA2  

$0  offset DMAx_ISR
$4  offset DMAx_IFCR
$8  offset DMAx_CCR1
$C  offset DMAx_CNDTR1
$10 offset DMAx_CPAR1
$14 offset DMAx_CMAR1
$1C offset DMAx_CCR2
$20 offset DMAx_CNDTR2
$24 offset DMAx_CPAR2
$28 offset DMAx_CMAR2
$30 offset DMAx_CCR3
$34 offset DMAx_CNDTR3
$38 offset DMAx_CPAR3
$3C offset DMAx_CMAR3
$44 offset DMAx_CCR4
$48 offset DMAx_CNDTR4
$4C offset DMAx_CPAR4
$50 offset DMAx_CMAR4
$58 offset DMAx_CCR5
$5C offset DMAx_CNDTR5
$60 offset DMAx_CPAR5
$64 offset DMAx_CMAR5
$6C offset DMAx_CCR6
$70 offset DMAx_CNDTR6
$74 offset DMAx_CPAR6
$78 offset DMAx_CMAR6
$80 offset DMAx_CCR7
$84 offset DMAx_CNDTR7
$88 offset DMAx_CPAR7
$8C offset DMAx_CMAR7
	
\ === Secure digital I/O interface
$40018000 constant SDIO  
SDIO $0 + constant SDIO_POWER
SDIO $4 + constant SDIO_CLKCR
SDIO $8 + constant SDIO_ARG
SDIO $C + constant SDIO_CMD
SDIO $10 + constant SDIO_RESPCMD
SDIO $14 + constant SDIO_RESPI1
SDIO $18 + constant SDIO_RESP2
SDIO $1C + constant SDIO_RESP3
SDIO $20 + constant SDIO_RESP4
SDIO $24 + constant SDIO_DTIMER
SDIO $28 + constant SDIO_DLEN
SDIO $2C + constant SDIO_DCTRL
SDIO $30 + constant SDIO_DCOUNT
SDIO $34 + constant SDIO_STA
SDIO $38 + constant SDIO_ICR
SDIO $3C + constant SDIO_MASK
SDIO $48 + constant SDIO_FIFOCNT
SDIO $80 + constant SDIO_FIFO

\ === Real Time Clock ===
$40002800 constant RTC  
RTC $0 + constant RTC_CRH
RTC $4 + constant RTC_CRL
RTC $8 + constant RTC_PRLH
RTC $C + constant RTC_PRLL
RTC $10 + constant RTC_DIVH
RTC $14 + constant RTC_DIVL
RTC $18 + constant RTC_CNTH
RTC $1C + constant RTC_CNTL
RTC $20 + constant RTC_ALRH
RTC $24 + constant RTC_ALRL

\ === Backup Registers ===
$40006C04 constant BKP  
BKP $0 + constant BKP_DR1
BKP $4 + constant BKP_DR2
BKP $8 + constant BKP_DR3
BKP $C + constant BKP_DR4
BKP $10 + constant BKP_DR5
BKP $14 + constant BKP_DR6
BKP $18 + constant BKP_DR7
BKP $1C + constant BKP_DR8
BKP $20 + constant BKP_DR9
BKP $24 + constant BKP_DR10
BKP $3C + constant BKP_DR11
BKP $40 + constant BKP_DR12
BKP $44 + constant BKP_DR13
BKP $48 + constant BKP_DR14
BKP $4C + constant BKP_DR15
BKP $50 + constant BKP_DR16
BKP $54 + constant BKP_DR17
BKP $58 + constant BKP_DR18
BKP $5C + constant BKP_DR19
BKP $60 + constant BKP_DR20
BKP $64 + constant BKP_DR21
BKP $68 + constant BKP_DR22
BKP $6C + constant BKP_DR23
BKP $70 + constant BKP_DR24
BKP $74 + constant BKP_DR25
BKP $78 + constant BKP_DR26
BKP $7C + constant BKP_DR27
BKP $80 + constant BKP_DR28
BKP $84 + constant BKP_DR29
BKP $88 + constant BKP_DR30
BKP $8C + constant BKP_DR31
BKP $90 + constant BKP_DR32
BKP $94 + constant BKP_DR33
BKP $98 + constant BKP_DR34
BKP $9C + constant BKP_DR35
BKP $A0 + constant BKP_DR36
BKP $A4 + constant BKP_DR37
BKP $A8 + constant BKP_DR38
BKP $AC + constant BKP_DR39
BKP $B0 + constant BKP_DR40
BKP $B4 + constant BKP_DR41
BKP $B8 + constant BKP_DR42
BKP $28 + constant BKP_RTCCR
BKP $2C + constant BKP_CR
BKP $30 + constant BKP_CSR

\ === Independent Watchdog
$40003000 constant IWDG  
IWDG $0 + constant IWDG_KR
IWDG $4 + constant IWDG_PR
IWDG $8 + constant IWDG_RLR
IWDG $C + constant IWDG_SR
        
	
\ === Window Watchdog ===
$40002C00 constant WWDG  
WWDG $0 + constant WWDG_CR
WWDG $4 + constant WWDG_CFR
WWDG $8 + constant WWDG_SR
        
	
\ === Timer ===
$40012C00 constant TIM1  
$40000000 constant TIM2  
$40000400 constant TIM3  
$40000800 constant TIM4  
$40000C00 constant TIM5  
$40001000 constant TIM6  
$40001400 constant TIM7  
$40013400 constant TIM8  
$40014C00 constant TIM9  
$40015000 constant TIM10  
$40015400 constant TIM11  
$40001800 constant TIM12  
$40001C00 constant TIM13  
$40002000 constant TIM14  

$0  offset TIM1_CR1
$4  offset TIM1_CR2
$8  offset TIM1_SMCR
$C  offset TIM1_DIER
$10 offset TIM1_SR
$14 offset TIM1_EGR
$18 offset TIM1_CCMR1_Output
$18 offset TIM1_CCMR1_Input
$1C offset TIM1_CCMR2_Output
$1C offset TIM1_CCMR2_Input
$20 offset TIM1_CCER
$24 offset TIM1_CNT
$28 offset TIM1_PSC
$2C offset TIM1_ARR
$34 offset TIM1_CCR1
$38 offset TIM1_CCR2
$3C offset TIM1_CCR3
$40 offset TIM1_CCR4
$48 offset TIM1_DCR
$4C offset TIM1_DMAR
$30 offset TIM1_RCR
$44 offset TIM1_BDTR
        
	
\ Inter-integrated circuit interface
$40005400 constant I2C1  
$40005800 constant I2C2  

$0  offset I2Cx_CR1
$4  offset I2Cx_CR2
$8  offset I2Cx_OAR1
$C  offset I2Cx_OAR2
$10 offset I2Cx_DR
$14 offset I2Cx_SR1
$18 offset I2Cx_SR2
$1C offset I2Cx_CCR
$20 offset I2Cx_TRISE
        
	
\ === Serial Periperal Interface ===
$40013000 constant SPI1  
$40003800 constant SPI2  
$40003C00 constant SPI3  

$0  offset SPIx_CR1
$4  offset SPIx_CR2
$8  offset SPIx_SR
$C  offset SPIx_DR
$10 offset SPIx_CRCPR
$14 offset SPIx_RXCRCR
$18 offset SPIx_TXCRCR
$1C offset SPIx_I2SCFGR
$20 offset SPIx_I2SPR
        
	
\ === UART ===
$40004C00 constant UART4  
$40013800 constant USART1  
$40004400 constant USART2  
$40004800 constant USART3  
$40005000 constant UART5  

$0  offset USARTx_SR
$4  offset USARTx_DR
$8  offset USARTx_BRR
$C  offset USARTx_CR1
$10 offset USARTx_CR2
$14 offset USARTx_CR3
$18 offset USARTx_GTPR

        
\ === Analog digital converter ===
$40012400 constant ADC1  
$40012800 constant ADC2  
$40013C00 constant ADC3  

$0  offset ADCx_SR
$4  offset ADCx_CR1
$8  offset ADCx_CR2
$C  offset ADCx_SMPR1
$10 offset ADCx_SMPR2
$14 offset ADCx_JOFR1
$18 offset ADCx_JOFR2
$1C offset ADCx_JOFR3
$20 offset ADCx_JOFR4
$24 offset ADCx_HTR
$28 offset ADCx_LTR
$2C offset ADCx_SQR1
$30 offset ADCx_SQR2
$34 offset ADCx_SQR3
$38 offset ADCx_JSQR
$3C offset ADCx_JDR1
$40 offset ADCx_JDR2
$44 offset ADCx_JDR3
$48 offset ADCx_JDR4
$4C offset ADCx_DR
        

\ === Controller area network ===
$40006400 constant CAN  
CAN $0 + constant CAN_CAN_MCR
CAN $4 + constant CAN_CAN_MSR
CAN $8 + constant CAN_CAN_TSR
CAN $C + constant CAN_CAN_RF0R
CAN $10 + constant CAN_CAN_RF1R
CAN $14 + constant CAN_CAN_IER
CAN $18 + constant CAN_CAN_ESR
CAN $1C + constant CAN_CAN_BTR
CAN $180 + constant CAN_CAN_TI0R
CAN $184 + constant CAN_CAN_TDT0R
CAN $188 + constant CAN_CAN_TDL0R
CAN $18C + constant CAN_CAN_TDH0R
CAN $190 + constant CAN_CAN_TI1R
CAN $194 + constant CAN_CAN_TDT1R
CAN $198 + constant CAN_CAN_TDL1R
CAN $19C + constant CAN_CAN_TDH1R
CAN $1A0 + constant CAN_CAN_TI2R
CAN $1A4 + constant CAN_CAN_TDT2R
CAN $1A8 + constant CAN_CAN_TDL2R
CAN $1AC + constant CAN_CAN_TDH2R
CAN $1B0 + constant CAN_CAN_RI0R
CAN $1B4 + constant CAN_CAN_RDT0R
CAN $1B8 + constant CAN_CAN_RDL0R
CAN $1BC + constant CAN_CAN_RDH0R
CAN $1C0 + constant CAN_CAN_RI1R
CAN $1C4 + constant CAN_CAN_RDT1R
CAN $1C8 + constant CAN_CAN_RDL1R
CAN $1CC + constant CAN_CAN_RDH1R
CAN $200 + constant CAN_CAN_FMR
CAN $204 + constant CAN_CAN_FM1R
CAN $20C + constant CAN_CAN_FS1R
CAN $214 + constant CAN_CAN_FFA1R
CAN $21C + constant CAN_CAN_FA1R
CAN $240 + constant CAN_F0R1
CAN $244 + constant CAN_F0R2
CAN $248 + constant CAN_F1R1
CAN $24C + constant CAN_F1R2
CAN $250 + constant CAN_F2R1
CAN $254 + constant CAN_F2R2
CAN $258 + constant CAN_F3R1
CAN $25C + constant CAN_F3R2
CAN $260 + constant CAN_F4R1
CAN $264 + constant CAN_F4R2
CAN $268 + constant CAN_F5R1
CAN $26C + constant CAN_F5R2
CAN $270 + constant CAN_F6R1
CAN $274 + constant CAN_F6R2
CAN $278 + constant CAN_F7R1
CAN $27C + constant CAN_F7R2
CAN $280 + constant CAN_F8R1
CAN $284 + constant CAN_F8R2
CAN $288 + constant CAN_F9R1
CAN $28C + constant CAN_F9R2
CAN $290 + constant CAN_F10R1
CAN $294 + constant CAN_F10R2
CAN $298 + constant CAN_F11R1
CAN $29C + constant CAN_F11R2
CAN $2A0 + constant CAN_F12R1
CAN $2A4 + constant CAN_F12R2
CAN $2A8 + constant CAN_F13R1
CAN $2AC + constant CAN_F13R2


\ === Digital to analog converter ===
$40007400 constant DAC  
DAC $0 + constant DAC_CR
DAC $4 + constant DAC_SWTRIGR
DAC $8 + constant DAC_DHR12R1
DAC $C + constant DAC_DHR12L1
DAC $10 + constant DAC_DHR8R1
DAC $14 + constant DAC_DHR12R2
DAC $18 + constant DAC_DHR12L2
DAC $1C + constant DAC_DHR8R2
DAC $20 + constant DAC_DHR12RD
DAC $24 + constant DAC_DHR12LD
DAC $28 + constant DAC_DHR8RD
DAC $2C + constant DAC_DOR1
DAC $30 + constant DAC_DOR2


\ === Debug ===
$E0042000 constant DBG  
DBG $0 + constant DBG_IDCODE
DBG $4 + constant DBG_CR


\ === CRC ===
$40023000 constant CRC  
CRC $0 + constant CRC_DR
CRC $4 + constant CRC_IDR
CRC $8 + constant CRC_CR
	

\ === Flash ===
$40022000 constant FLASH  
FLASH $0 + constant FLASH_ACR
FLASH $4 + constant FLASH_KEYR
FLASH $8 + constant FLASH_OPTKEYR
FLASH $C + constant FLASH_SR
FLASH $10 + constant FLASH_CR
FLASH $14 + constant FLASH_AR
FLASH $1C + constant FLASH_OBR
FLASH $20 + constant FLASH_WRPR
        
	
\ === Nested Vector Interrupt Controller
$E000E000 constant NVIC  
NVIC $4 + constant NVIC_ICTR
NVIC $F00 + constant NVIC_STIR
NVIC $100 + constant NVIC_ISER0
NVIC $104 + constant NVIC_ISER1
NVIC $180 + constant NVIC_ICER0
NVIC $184 + constant NVIC_ICER1
NVIC $200 + constant NVIC_ISPR0
NVIC $204 + constant NVIC_ISPR1
NVIC $280 + constant NVIC_ICPR0
NVIC $284 + constant NVIC_ICPR1
NVIC $300 + constant NVIC_IABR0
NVIC $304 + constant NVIC_IABR1
NVIC $400 + constant NVIC_IPR0
NVIC $404 + constant NVIC_IPR1
NVIC $408 + constant NVIC_IPR2
NVIC $40C + constant NVIC_IPR3
NVIC $410 + constant NVIC_IPR4
NVIC $414 + constant NVIC_IPR5
NVIC $418 + constant NVIC_IPR6
NVIC $41C + constant NVIC_IPR7
NVIC $420 + constant NVIC_IPR8
NVIC $424 + constant NVIC_IPR9
NVIC $428 + constant NVIC_IPR10
NVIC $42C + constant NVIC_IPR11
NVIC $430 + constant NVIC_IPR12
NVIC $434 + constant NVIC_IPR13
NVIC $438 + constant NVIC_IPR14


\ === USB ===
$40005C00 constant USB  
USB $0 + constant USB_EP0R
USB $4 + constant USB_EP1R
USB $8 + constant USB_EP2R
USB $C + constant USB_EP3R
USB $10 + constant USB_EP4R
USB $14 + constant USB_EP5R
USB $18 + constant USB_EP6R
USB $1C + constant USB_EP7R
USB $40 + constant USB_CNTR
USB $44 + constant USB_ISTR
USB $48 + constant USB_FNR
USB $4C + constant USB_DADDR
USB $50 + constant USB_BTABLE
        
compiletoram
