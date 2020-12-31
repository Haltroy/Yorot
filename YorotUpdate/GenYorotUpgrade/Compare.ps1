$Folder1 = ".\vOld\"
$Folder2 = ".\VNew\"
$F1 = '.\\vOld\\'
$F2 = '.\\VNew\\'
$Diff1 = ".\vOld.txt"
$Diff2 = ".\vNew.txt"
$Out = '.\Result.txt'

Get-ChildItem $Folder1 -Recurse | Get-FileHash | Select Path, Hash | export-csv $Diff1 -Delimiter "`t" -NoTypeInformation
(gc -path $Diff1 -raw) -replace $F1, "" | Out-File $Diff1

Get-ChildItem $Folder2 -Recurse | Get-FileHash | Select Path, Hash | export-csv $Diff2 -Delimiter "`t" -NoTypeInformation
(gc -path $Diff2 -raw) -replace $F2, "" | Out-File $Diff2

Compare-Object -ReferenceObject $(Get-Content $Diff1) -DifferenceObject $(Get-Content $Diff2) | fl > $Out

(gc -path $Out -raw) -replace '\=\>', "Different" | Out-File $Out

(gc -path $Out -raw) -replace '\<\=', "Different" | Out-File $Out