# Specimen Upload Data Model
Specimen upload data model.

>[!NOTE]
> All exact dates are hidden and protected. Relative dates are shown instead, if calculation was possible.

**`id`*** - Specimen identifier.
- Note: Specimen identifiers are namespaced and should be unique for it's donor across all specimens of the same type.
- Type: _String_
- Limitations: Maximum length 255
- Example: `"Specimen1"`

**`donor_id`*** - Specimen donor identifier.
- Type: _String_
- Limitations: Maximum length 255
- Example: `"Donor1"`

**`parent_id`** - Parent specimen identifier.
- Type: _String_
- Limitations: Maximum length 255
- Example: `"Material1"`

**`parent_type`** - Parent specimen type.
- Type: _String_
- Possible values: `"Material"`, `"Line"`, `"Organoid"`, `"Xenograft"`
- Example: `"Material"`

**`creation_date`** - Date when specimen was created.
- Type: _String_
- Format: "YYYY-MM-DD"
- Limitations: Only either `creation_date` or `creation_day` can be set at once, not both
- Example: `"2020-02-05"`

**`creation_day`** - Relative number of days since diagnosis statement when specimen was created.
- Type: _Number_
- Limitations: Integer, greater or equal to 1, only either `creation_date` or `creation_day` can be set at once, not both
- Example: `36`

**`material`** - Material data (if specimen is a donor material).
- Type: _Object([Material](api-models-base-material.md))_
- Limitations - Only either `material`, `line`, `organoid` or `xenograft` can be set at once.
- Example: `{...}`

**`line`** - Cell line data (if specimen is a cell line).
- Type: _Object([CellLine](api-models-base-line.md))_
- Limitations - Only either `material`, `line`, `organoid` or `xenograft` can be set at once.
- Example: `{...}`

**`organoid`** - Organoid data (if specimen is an organoid).
- Type: _Object([Organoid](api-models-base-organoid.md))_
- Limitations - Only either `material`, `line`, `organoid` or `xenograft` can be set at once.
- Example: `{...}`

**`xenograft`** - Xenograft data (if specimen is a xenograft).
- Type: _Object([Xenograft](api-models-base-xenograft.md))_
- Limitations - Only either `material`, `line`, `organoid` or `xenograft` can be set at once.
- Example: `{...}`

**`molecular_data`** - Specimen molecular data.
- Type: _Object([MolecularData](api-models-base-molecular.md))_
- Example: `{...}`

**`interventions`** - Specimen interventions data.
- Type: _Array_
- Element type: _Object([Intervention](api-models-base-intervention.md))_
- Example: `[{...}, {...}]`

**`drug_screenings`** - Specimen drug screening data.
- Type: _Array_
- Element type: _Object([DrugScreeningData](api-models-base-drug.md))_
- Example: `[{...}, {...}]`

##
**`*`** - Required fields
