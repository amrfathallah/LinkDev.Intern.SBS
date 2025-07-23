import { getTestBed } from '@angular/core/testing';
import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ResourceType } from '../../enums/ResourceType.enum';
import { Resource } from '../../models/Resource.model';

export interface ResourceDialogData {
  resource?: Resource;
  isEdit: boolean;
}

@Component({
  selector: 'app-resource-dialog',
  templateUrl: './resource-dialog.component.html',
  styleUrls: ['./resource-dialog.component.css']
})
export class ResourceDialogComponent implements OnInit {
  resourceForm: FormGroup;
  ResourceType = ResourceType;

  constructor(
    public dialogRef: MatDialogRef<ResourceDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ResourceDialogData,
    private fb: FormBuilder
  ) {
    this.resourceForm = this.createForm();
  }

  ngOnInit(): void {
    if (this.data.isEdit && this.data.resource) {
      this.populateForm(this.data.resource);
    }
  }

  // TODO: figure out what is the best practices for making forms
  private createForm(): FormGroup {
    return this.fb.group({
      name: ['', [Validators.required]],
      type: [1, this.data.isEdit ? [] : [Validators.required]],
      capacity: [1, [Validators.required, Validators.min(1)]],
      openAt: ['09:00', Validators.required],
      closeAt: ['17:00', Validators.required],
      active: [true]
    });
  }

  private populateForm(resource: Resource): void {
    this.resourceForm.patchValue({
      name: resource.name,
      type: resource.typeId,
      capacity: resource.capacity,
      openAt: resource.openAt.substring(0, 5),
      closeAt: resource.closeAt.substring(0, 5),
      active: resource.active
    });
  }

  onSave(): void {
    if (this.resourceForm.valid) {
      const formValue = this.resourceForm.value;

      const resourceData: Partial<Resource> = {
        name: formValue.name,
        capacity: formValue.capacity,

        // XXX: it's a hack, should be handled in a better way to match the expected format
        openAt: (formValue.openAt) + ":00",
        closeAt:(formValue.closeAt) + ":00",
        typeId: formValue.type,
        active: formValue.active,
      };

      if (this.data.isEdit && this.data.resource) {
        resourceData.typeId = this.data.resource.typeId;
        resourceData.id = this.data.resource.id;
      } else {
        resourceData.typeId = formValue.type as ResourceType;
      }
      this.dialogRef.close(resourceData);
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
