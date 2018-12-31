#!/bin/bash
#(c)copyright 2018 by Gerald Wodni <gerald.wodni@gmail.com>

DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
CREATEFONT="${DIR}/createFont.py"

FONTDIR="${DIR}/../media/fonts/"
FORTHFONTDIR="${DIR}/../src/fonts/"

# create fonts
for FILE in `ls -1 ${FONTDIR}`; do
    FONTFILE="${FONTDIR}${FILE}"
    FORTHFILE="${FORTHFONTDIR}${FILE}"
    #echo "${FONTFILE} -> ${FORTHFILE}"
    ${CREATEFONT} font ${FONTFILE} ${FORTHFILE}
done
