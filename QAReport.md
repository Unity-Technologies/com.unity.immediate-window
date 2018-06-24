# Quality Report
Use this file to outline the test strategy for this package.

## QA Owner: [Pedro Albuquerque](mailto:pedroa@unity3d.com)
* __Note:__ No QA for this release

## UX Owner: [Maddalena Vismara](mailto:maddalena@unity3d.com)

## Test strategy
* A link to the Test Plan (Test Rails, other)
* Results from the package's editor and runtime test suite.

```
./utr.pl --suite=editor --testprojects=PackageManagerUI
Completed tests for project PackageManagerUI ExitCode:0
Overall result: PASS
Total Tests run: 54, Passed: 54, Failures: 0, Errors: 0, Inconclusives: 0
Total not run : 0, Invalid: 0, Ignored: 0, Skipped: 0
```

* Link to automated test results (if any)

```
- Verify_Package_Exists: Passed
- Verify_Package_Exists_Extra: Passed
- PackageCollectionTests.AddPackageInfo_PackagesChangeEventIsPropagated: Passed
- PackageCollectionTests.AddPackageInfos_PackagesChangeEventIsPropagated: Passed
- PackageCollectionTests.ClearPackages_PackagesChangeEventIsPropagated: Passed
- PackageCollectionTests.Constructor_Instance_FilterIsLocal: Passed
- PackageCollectionTests.Constructor_Instance_PackageInfosIsEmpty: Passed
- PackageCollectionTests.SetFilter_WhenFilterChange_FilterChangeEventIsPropagated: Passed
- PackageCollectionTests.SetFilter_WhenFilterChange_FilterIsChanged: Passed
- PackageCollectionTests.SetFilter_WhenFilterChangeNoRefresh_PackagesChangeEventIsNotPropagated: Passed
- PackageCollectionTests.SetFilter_WhenNoFilterChange_FilterChangeEventIsNotPropagated: Passed
- PackageCollectionTests.SetFilter_WhenNoFilterChangeNoRefresh_PackagesChangeEventIsNotPropagated: Passed
- PackageCollectionTests.SetFilter_WhenNoFilterChangeRefresh_PackagesChangeEventIsNotPropagated: Passed
- PackageCollectionTests.SetPackageInfos_PackagesChangeEventIsPropagated: Passed
- PackageDetailsTests.Show_CorrectPackage: Passed
- PackageDetailsTests.Show_CorrectTag: Passed
- PackageInfoTests.HasTag_WhenPackageVersionTagIsAnyCase_ReturnsTrue: Passed
- PackageInfoTests.HasTag_WhenPreReleasePackageVersionTagWithPreReleaseName_ReturnsTrue: Passed
- PackageInfoTests.VersionWithoutTag_WhenVersionContainsTag_ReturnsVersionOnly: Passed
- PackageInfoTests.VersionWithoutTag_WhenVersionDoesNotContainTag_ReturnsVersionOnly: Passed
- PackageManagerWindowTests.When_Default_FirstPackageUIElement_HasSelectedClass: Passed
- PackageManagerWindowTests.When_Default_PackageGroupsCollapsedState: Passed
- PackageManagerWindowTests.When_Default_PackageGroupsCollapsedState_Has_NoChildren: Passed
- PackageManagerWindowTests.When_Filter_Changes_Shows_Correct_List: Passed
- PackageManagerWindowTests.When_PackageCollection_Changes_PackageList_Updates: Passed
- PackageManagerWindowTests.When_PackageCollection_Remove_Fails_PackageLists_NotUpdated: Passed
- PackageManagerWindowTests.When_PackageCollection_Remove_PackageLists_Updated: Passed
- PackageManagerWindowTests.When_PackageCollection_Update_Fails_Package_Stay_Current: Passed
- PackageManagerWindowTests.When_PackageCollection_Updates_PackageList_Updates: Passed
- PackageTests.Add_WhenPackageInfoIsCurrent_AddOperationIsNotCalled: Passed
- PackageTests.Add_WhenPackageInfoIsNotCurrent_AddOperationIsCalled: Passed
- PackageTests.CanBeRemoved_WhenNotPackageManagerUIPackage_ReturnsTrue: Passed
- PackageTests.CanBeRemoved_WhenPackageManagerUIPackage_ReturnsFalse: Passed
- PackageTests.Constructor_WithEmptyPackageInfos_ThrowsException: Passed
- PackageTests.Constructor_WithEmptyPackageName_ThrowsException: Passed
- PackageTests.Constructor_WithMultiplePackagesInfo_VersionsCorrespond: Passed
- PackageTests.Constructor_WithNullPackageInfos_ThrowsException: Passed
- PackageTests.Constructor_WithNullPackageName_ThrowsException: Passed
- PackageTests.Constructor_WithOnePackageInfo_CurrentIsFirstVersion: Passed
- PackageTests.Constructor_WithOnePackageInfo_LatestAndCurrentAreEqual: Passed
- PackageTests.Constructor_WithOnePackageInfo_LatestIsLastVersion: Passed
- PackageTests.Constructor_WithTwoPackageInfos_CurrentIsFirstVersion: Passed
- PackageTests.Constructor_WithTwoPackageInfos_LatestIsLastVersion: Passed
- PackageTests.Constructor_WithTwoPackagesInfo_LatestAndCurrentAreNotEqual: Passed
- PackageTests.Display_WhenCurrentAndLatest_ReturnsLatest: Passed
- PackageTests.Display_WhenCurrentIsNotNull_ReturnsCurrent: Passed
- PackageTests.Display_WhenCurrentIsNull_ReturnsLatest: Passed
- PackageTests.DocumentationLink_ReturnsNotEmptyString: Passed
- PackageTests.Name_ReturnsExpectedValue: Passed
- PackageTests.Remove_RemoveOperationIsCalled: Passed
- PackageTests.Update_WhenCurrentIsLatest_AddOperationIsNotCalled: Passed
- PackageTests.Update_WhenCurrentIsNotLatest_AddOperationIsCalled: Passed
- PackageTests.Versions_WhenOrderedPackageInfo_ReturnsOrderedValues: Passed
- PackageTests.Versions_WhenUnorderedPackageInfo_ReturnsOrderedValues: Passed
```

* Manual test results, [Package Manager UI - QA Matrix](https://docs.google.com/a/unity3d.com/spreadsheets/d/1Vh4x1Tjk1Pvv9NER6mFShBIwNvN6wOtjVlo89OTfunY/edit?usp=sharing)

## Package Status
* package stability
	* Stable-ish
* known issues
	* Modifying the `manifest.json` by hand doesn't update the package list. You need to either re-open the window or change filters to force an update.
	* Built-In packages cannot be enabled/disabled
	* Package Manager UI is not taking care whether or not the package you want to install/update has no compilation error.
	* Package Manager UI refreshes on Domain Reload and go back to Project tab
	* Using a cache server may not update your assemblies properly, please turn it off
