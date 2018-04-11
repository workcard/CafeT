###################################################################################################
#
# Deal with the command line - We need two variables passed in
#     CategoryName     - Name of the Performance Counter Category that we are going to create
#     $CounterFilePath - Path to the XML file that contains the names of the counters to create
###################################################################################################

Param(
  [string]$CategoryName,
  [string]$CounterFilePath
)

# Check if we got the proper command line arguments
if ( $CategoryName -eq '' -or $CounterFilePath -eq '')
{
     Write-Host "Usage: CreatePerformanceCounters.ps1 -CategoryName <category name> -CounterFilePath <path to counter xml file>"
    exit -1
}

# Make sure the file exists
$fileExists = Test-Path $CounterFilePath
if ( !$fileExists)
{
    Write-Host "Unable to find counters file at $CounterFilePath"
    exit -2
}


###################################################################################################
#
# Define some shorthand variables for .NET class types used throughout the script
#     --These make the code easier to read
#
###################################################################################################
$PerfCounterType = [System.Diagnostics.PerformanceCounterType]
$PerfCounterCategory = [System.Diagnostics.PerformanceCounterCategory]
$PerfCounterCategoryType = [System.Diagnostics.PerformanceCounterCategoryType]

###################################################################################################
#
# The file will have string representations of the Performance Counter Types.  We need to convert
#     those into enum values, so the following hashtable will help us do that
#
###################################################################################################
$counterTypesLookup = @{}

$CounterTypeNames = [Enum]::GetNames( $PerfCounterType )
foreach ($TypeName in $CounterTypeNames)
{
    $CounterTypeValue = $PerfCounterType::$TypeName
    $counterTypesLookup[$TypeName] = $CounterTypeValue
}


###################################################################################################
#
# Read the XML file so we can process the counters
#
###################################################################################################
[xml]$counterList = Get-Content $CounterFilePath


# This is the collection that counters must be added to in order to be created
$counterCollection = new-object System.Diagnostics.CounterCreationDataCollection


foreach( $counter in $counterList.Counters.Counter) 
{ 
    $createData = new-object  System.Diagnostics.CounterCreationData
    $createData.CounterName = $counter.Name
    $createData.CounterHelp = $counter.Description
    
    $counterType = $counterTypesLookup[$counter.Type]

    if ( !$counterType )
    {
        throw "Counter Type $counterType is not known"
    }

    $createData.CounterType = $counterType
    $itemCount = $counterCollection.Add($createData)
}


 
$exists = $PerfCounterCategory::Exists($CategoryName)
if ($exists)
{
    Write-Host "Category $CategoryName already exists.  Deleting so it can be recreated"
    $PerfCounterCategory::Delete($CategoryName)
}
 
$CategoryHelp = "Counters to track performance of ASP.NET MVC and WebAPI Applications"

try
{
    $PerfCounterCategory::Create($CategoryName, $CategoryHelp, $PerfCounterCategoryType::MultiInstance, $counterCollection)

    Write-Host "Performance Counters Created Successfully"
}
catch
{
    "Error creating performance counters $($error[0])"
    exit
}