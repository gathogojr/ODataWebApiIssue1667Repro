<!-- markdownlint-disable MD002 MD041 -->

## Objective
Investigate issues [#1667](https://github.com/OData/odata.net/issues/1667) and [#1162](https://github.com/OData/odata.net/issues/1162) reported on [ODL](https://github.com/OData/odata.net) project.

Issue affecting filtering ($filter) and ordering ($orderby) in $expand applied to Edm functions

### Cases highlighted:
 - [ ] https://graph.microsoft.com/v1.0/me/mailFolders/inbox/messages/delta?$expand=singleValueExtendedProperties($filter=id eq 'value')
 - [ ] https://graph.microsoft.com/v1.0/me/activities/recent?$expand=historyItems($filter=lastModifiedDateTime%20gt%202018-01-22T21:45:00.347Z%20and%20lastModifiedDateTime%20lt%202018-01-22T22:00:00.347Z)
 - [ ] https://graph.microsoft.com/v1.0/me/drive/root/delta?$expand=microsoft.graph.driveItem/children($filter=id eq ‘x’)
 - [ ] https://graph.microsoft.com/v1.0/me/drive/root/delta?$expand=microsoft.graph.driveItem/children($orderby=id)
- Where `delta` and `recent` are both bound Edm functions

### Assemblies in attempt to reproduce the issue:
- Microsoft.AspNetCore.OData 7.4.1
- Microsoft.OData.Core 7.6.1
- EntityFramework 6.4.4

**To set up database, execute the following commands against ODataWebApiIssue1667Repro.Lib project (using Package Manager Console)**
- Enable-Migrations
- Update-Database -TargetMigration:0
- Add-Migration InitialMigration -Force
- Update-Database

## Verified Scenarios
Verified the following scenarios work okay in an OData Web API service

### Where:
- `Projects` is an entity set
- `Milestones` is a collection navigation property on `Project` entity
- `Tasks` is a collection navigation property on `Milestone` entity

 - [x] http://localhost:1031/odata/Projects
 - [x] http://localhost:1031/odata/Projects?$expand=Milestones
 - [x] http://localhost:1031/odata/Projects?$expand=Milestones($select=Name)
 - [x] http://localhost:1031/odata/Projects?$expand=Milestones($orderby=Name)
 - [x] http://localhost:1031/odata/Projects?$expand=Milestones($filter=contains(Name, 'Install'))
 - [x] http://localhost:1031/odata/Projects?$expand=Milestones($expand=Tasks)
 - [x] http://localhost:1031/odata/Projects?$expand=Milestones($expand=Tasks($select=Description))
 - [x] http://localhost:1031/odata/Projects?$expand=Milestones($expand=Tasks($orderby=Description))
 - [x] http://localhost:1031/odata/Projects?$expand=Milestones($expand=Tasks($filter=contains(Description, 'Install')))

### Where:
- `KeyProjects` is an `Edm` function bound to `Project` collection
- `KeyProjects` returns collection from entity set

 - [x] http://localhost:1031/odata/Projects/KeyProjects
 - [x] http://localhost:1031/odata/Projects/KeyProjects?$expand=Milestones
 - [x] http://localhost:1031/odata/Projects/KeyProjects?$expand=Milestones($select=Name)
 - [x] http://localhost:1031/odata/Projects/KeyProjects?$expand=Milestones($orderby=Name)
 - [x] http://localhost:1031/odata/Projects/KeyProjects?$expand=Milestones($filter=contains(Name, 'Install'))
 - [x] http://localhost:1031/odata/Projects/KeyProjects?$expand=Milestones($expand=Tasks)
 - [x] http://localhost:1031/odata/Projects/KeyProjects?$expand=Milestones($expand=Tasks($select=Description))
 - [x] http://localhost:1031/odata/Projects/KeyProjects?$expand=Milestones($expand=Tasks($orderby=Description))
 - [x] http://localhost:1031/odata/Projects/KeyProjects?$expand=Milestones($expand=Tasks($filter=contains(Description, 'Install')))

### Where:
- `Company` is a singleton
- `Projects` is an `Edm` function bound to `Company` singleton
- `Projects` returns collection from entity set
- Expansion Depth of 4 (deeply nested `$expand`)

 - [x] http://localhost:1031/odata/Company/Projects
 - [x] http://localhost:1031/odata/Company/Projects?$expand=Manager
 - [x] http://localhost:1031/odata/Company/Projects?$expand=Manager($expand=Reports)
 - [x] http://localhost:1031/odata/Company/Projects?$expand=Manager($expand=Reports($select=Name))
 - [x] http://localhost:1031/odata/Company/Projects?$expand=Manager($expand=Reports($orderby=Name))
 - [x] http://localhost:1031/odata/Company/Projects?$expand=Manager($expand=Reports($filter=contains(Name, 'er')))
 - [x] http://localhost:1031/odata/Company/Projects?$expand=Manager($expand=Reports($expand=Tasks))
 - [x] http://localhost:1031/odata/Company/Projects?$expand=Manager($expand=Reports($expand=Tasks($select=Description)))
 - [x] http://localhost:1031/odata/Company/Projects?$expand=Manager($expand=Reports($expand=Tasks($orderby=Description)))
 - [x] http://localhost:1031/odata/Company/Projects?$expand=Manager($expand=Reports($expand=Tasks($filter=contains(Description, 'Install'))))

### With:
- Qualified entity type names included in the Uri

 - [x] http://localhost:1031/odata/Company/Projects?$expand=Repro.Lib.Models.Project/Manager
 - [x] http://localhost:1031/odata/Company/Projects?$expand=Repro.Lib.Models.Project/Manager($expand=Repro.Lib.Models.Employee/Reports)
 - [x] http://localhost:1031/odata/Company/Projects?$expand=Repro.Lib.Models.Project/Manager($expand=Repro.Lib.Models.Employee/Reports($select=Name))
 - [x] http://localhost:1031/odata/Company/Projects?$expand=Repro.Lib.Models.Project/Manager($expand=Repro.Lib.Models.Employee/Reports($orderby=Name))
 - [x] http://localhost:1031/odata/Company/Projects?$expand=Repro.Lib.Models.Project/Manager($expand=Repro.Lib.Models.Employee/Reports($filter=contains(Name, 'er')))
 - [x] http://localhost:1031/odata/Company/Projects?$expand=Repro.Lib.Models.Project/Manager($expand=Repro.Lib.Models.Employee/Reports($expand=Repro.Lib.Models.Employee/Tasks))
 - [x] http://localhost:1031/odata/Company/Projects?$expand=Repro.Lib.Models.Project/Manager($expand=Repro.Lib.Models.Employee/Reports($expand=Repro.Lib.Models.Employee/Tasks($select=Description)))
 - [x] http://localhost:1031/odata/Company/Projects?$expand=Repro.Lib.Models.Project/Manager($expand=Repro.Lib.Models.Employee/Reports($expand=Repro.Lib.Models.Employee/Tasks($orderby=Description)))
 - [x] http://localhost:1031/odata/Company/Projects?$expand=Repro.Lib.Models.Project/Manager($expand=Repro.Lib.Models.Employee/Reports($expand=Repro.Lib.Models.Employee/Tasks($filter=contains(Description, 'Install'))))
 
 ## Conclusion
 It does seem like ODL is behaving as expected and $filter nested in $expand applied to Edm function does appear to be working okay.
 