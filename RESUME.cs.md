---
schema_version: 2
type: library
file_count: 197
delete_recommendation_percent: 5
generated_date: 2026-09-30
generated_time: 15:11:03
---

## Description

Indexer Visual Studio solutions a projektů v nich, vyčleněný z monolitu `SunamoDevCode`. Prochází složky se solutions, čte git informace (`GitHelper`) a serializuje strukturu složek, projektů a solutions pro další zpracování jinými Sunamo nástroji.
Publikovaný `PackageId` je `SunamoSlnIndexer` (adresář zůstal `SunamoSolutionsIndexer`). Balíček je self-contained: kód dříve referencovaného balíčku DevCodeBase je zkopírován do `Internal\` jako internal a jiné Sunamo balíčky nereferencuje.
