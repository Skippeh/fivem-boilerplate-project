param(
    # Template file name
    [Parameter(
        Position = 0,
        Mandatory = $true,
        ParameterSetName = "LiteralPath",
        HelpMessage = "The path to the __resource.lua template file."
    )]
    [String]
    $TemplateFileName,
    # Output file name
    [Parameter(
        Position = 1,
        Mandatory = $true,
        ParameterSetName = "LiteralPath",
        HelpMessage = "The output path of the __resource.lua file."
    )]
    [String]
    $OutputFileName,
    # Shared assembly file name
    [Parameter(
        Position = 2,
        Mandatory = $true,
        HelpMessage = "The name of the shared assembly file."
    )]
    [String]
    $SharedFileName,
    # Client assembly file name
    [Parameter(
        Position = 3,
        Mandatory = $true,
        HelpMessage = "The name of the client assembly file."
    )]
    [String]
    $ClientFileName,
    # Server assembly file name
    [Parameter(
        Position = 4,
        Mandatory = $true,
        HelpMessage = "The name of the server assembly file."
    )]
    [String]
    $ServerFileName
)

[IO.Directory]::SetCurrentDirectory((Get-Item -Path "./").FullName)

function GenerateResourceFileContents(
    [String]$templateFileName,
    [String]$sharedFileName,
    [String]$clientFileName,
    [String]$serverFileName
)
{
    $fileContents = [IO.File]::ReadAllText($templateFileName)
    $fileContents = $fileContents.Replace("{0}", $sharedFileName)
    $fileContents = $fileContents.Replace("{1}", $clientFileName)
    $fileContents = $fileContents.Replace("{2}", $serverFileName)
    return $fileContents
}

$TemplateContents = GenerateResourceFileContents $TemplateFileName $SharedFileName $ClientFileName $ServerFileName
[IO.File]::WriteAllText($OutputFileName, $TemplateContents)
exit 0