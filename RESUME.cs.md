---
schema_version: 1
type: library
file_count: 44
delete_recommendation_percent: 5
generated_date: 2026-09-29
---

## Description

Indexer Visual Studio solutions a projektů v nich, vyčleněný z monolitu `SunamoDevCode`.

Prochází složky se solutions, čte git informace (`GitHelper`) a serializuje strukturu složek/projektů/solutions pro další zpracování jinými Sunamo nástroji.

Publikovaný `PackageId` je `SunamoSlnIndexer` (přejmenováno 2026-06-21 kvůli rezervovanému názvu na NuGetu, adresář zůstal `SunamoSolutionsIndexer`). Publikováno na NuGet 2026-09-29.
