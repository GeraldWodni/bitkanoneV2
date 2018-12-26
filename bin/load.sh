#!/bin/bash
# load mecrisp (Forth System/Firmware) onto STlink
# on debian st-flash in contained in stlink-tools `apt-get install stlink-tools`
st-flash erase
st-flash write mecrisp-stellaris-stm32f103.bin 0x8000000
