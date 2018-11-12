#!/bin/bash

# Arguments:
# - $1 = template file
# - $2 = shared file name
# - $3 = client file name
# - $4 = server file name
function GenerateResourceFileContents () {
    Contents=$( cat $1 )

    Contents=${Contents//"{0}"/$2}
    Contents=${Contents//"{1}"/$3}
    Contents=${Contents//"{2}"/$4}
    
    echo $Contents
    return 0
}

function PrintUsage () {
    echo Usage:
    echo $0 TemplateFile OutputFileName SharedFileName ClientFileName ServerFileName
    echo
    echo Example:
    echo $0 "resource_template.lua ~/fivem_server/server-data/resources/[dev]/MyResource/__resource.lua MyResourceShared.net.dll MyResourceClient.net.dll MyResourceServer.net.dll"

    return 0
} >&2

TemplateFile=$1
OutputFileName=$2
SharedFileName=$3
ClientFileName=$4
ServerFileName=$5

if [ -z $TemplateFile ]
then
    echo "Template file argument missing"
    $( PrintUsage )
    exit 1
fi

if [ -z $OutputFileName ]
then
    echo "Output filename argument missing"
    $( PrintUsage )
    exit 1
fi

if [ -z $SharedFileName ]
then
    echo "Shared filename argument missing"
    $( PrintUsage )
    exit 1
fi

if [ -z $ClientFileName ]
then
    echo "Client filename argument missing"
    $( PrintUsage )
    exit 1
fi

if [ -z $ServerFileName ]
then
    echo "Server filename argument missing"
    $( PrintUsage )
    exit 1
fi

TemplateContents=$( GenerateResourceFileContents $TemplateFile $SharedFileName $ClientFileName $ServerFileName )
echo $TemplateContents > $OutputFileName
exit 0