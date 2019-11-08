using Microsoft.EntityFrameworkCore.Migrations;

namespace DiscImageChef.Server.Migrations
{
    public partial class SetAllOnDeleteSetNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ata_TestedMedia_ReadCapabilitiesId",
                table: "Ata");

            migrationBuilder.DropForeignKey(
                name: "FK_BlockDescriptor_ScsiMode_ScsiModeId",
                table: "BlockDescriptor");

            migrationBuilder.DropForeignKey(
                name: "FK_DensityCode_SscSupportedMedia_SscSupportedMediaId",
                table: "DensityCode");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Ata_ATAId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Ata_ATAPIId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_CdOffsets_CdOffsetId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_FireWire_FireWireId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_MmcSd_MultiMediaCardId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Pcmcia_PCMCIAId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Scsi_SCSIId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_MmcSd_SecureDigitalId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Usb_USBId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_DeviceStats_Devices_ReportId",
                table: "DeviceStats");

            migrationBuilder.DropForeignKey(
                name: "FK_Mmc_MmcFeatures_FeaturesId",
                table: "Mmc");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Ata_ATAId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Ata_ATAPIId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_FireWire_FireWireId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_MmcSd_MultiMediaCardId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Pcmcia_PCMCIAId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Scsi_SCSIId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_MmcSd_SecureDigitalId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Usb_USBId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Scsi_ScsiMode_ModeSenseId",
                table: "Scsi");

            migrationBuilder.DropForeignKey(
                name: "FK_Scsi_Mmc_MultiMediaDeviceId",
                table: "Scsi");

            migrationBuilder.DropForeignKey(
                name: "FK_Scsi_TestedMedia_ReadCapabilitiesId",
                table: "Scsi");

            migrationBuilder.DropForeignKey(
                name: "FK_Scsi_Ssc_SequentialDeviceId",
                table: "Scsi");

            migrationBuilder.DropForeignKey(
                name: "FK_ScsiPage_Scsi_ScsiId",
                table: "ScsiPage");

            migrationBuilder.DropForeignKey(
                name: "FK_ScsiPage_ScsiMode_ScsiModeId",
                table: "ScsiPage");

            migrationBuilder.DropForeignKey(
                name: "FK_SscSupportedMedia_Ssc_SscId",
                table: "SscSupportedMedia");

            migrationBuilder.DropForeignKey(
                name: "FK_SscSupportedMedia_TestedSequentialMedia_TestedSequentialMedi~",
                table: "SscSupportedMedia");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportedDensity_Ssc_SscId",
                table: "SupportedDensity");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportedDensity_TestedSequentialMedia_TestedSequentialMedia~",
                table: "SupportedDensity");

            migrationBuilder.DropForeignKey(
                name: "FK_TestedMedia_Ata_AtaId",
                table: "TestedMedia");

            migrationBuilder.DropForeignKey(
                name: "FK_TestedMedia_Chs_CHSId",
                table: "TestedMedia");

            migrationBuilder.DropForeignKey(
                name: "FK_TestedMedia_Chs_CurrentCHSId",
                table: "TestedMedia");

            migrationBuilder.DropForeignKey(
                name: "FK_TestedMedia_Mmc_MmcId",
                table: "TestedMedia");

            migrationBuilder.DropForeignKey(
                name: "FK_TestedMedia_Scsi_ScsiId",
                table: "TestedMedia");

            migrationBuilder.DropForeignKey(
                name: "FK_TestedSequentialMedia_Ssc_SscId",
                table: "TestedSequentialMedia");

            migrationBuilder.AddForeignKey(
                name: "FK_Ata_TestedMedia_ReadCapabilitiesId",
                table: "Ata",
                column: "ReadCapabilitiesId",
                principalTable: "TestedMedia",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_BlockDescriptor_ScsiMode_ScsiModeId",
                table: "BlockDescriptor",
                column: "ScsiModeId",
                principalTable: "ScsiMode",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_DensityCode_SscSupportedMedia_SscSupportedMediaId",
                table: "DensityCode",
                column: "SscSupportedMediaId",
                principalTable: "SscSupportedMedia",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Ata_ATAId",
                table: "Devices",
                column: "ATAId",
                principalTable: "Ata",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Ata_ATAPIId",
                table: "Devices",
                column: "ATAPIId",
                principalTable: "Ata",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_CdOffsets_CdOffsetId",
                table: "Devices",
                column: "CdOffsetId",
                principalTable: "CdOffsets",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_FireWire_FireWireId",
                table: "Devices",
                column: "FireWireId",
                principalTable: "FireWire",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_MmcSd_MultiMediaCardId",
                table: "Devices",
                column: "MultiMediaCardId",
                principalTable: "MmcSd",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Pcmcia_PCMCIAId",
                table: "Devices",
                column: "PCMCIAId",
                principalTable: "Pcmcia",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Scsi_SCSIId",
                table: "Devices",
                column: "SCSIId",
                principalTable: "Scsi",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_MmcSd_SecureDigitalId",
                table: "Devices",
                column: "SecureDigitalId",
                principalTable: "MmcSd",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Usb_USBId",
                table: "Devices",
                column: "USBId",
                principalTable: "Usb",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceStats_Devices_ReportId",
                table: "DeviceStats",
                column: "ReportId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Mmc_MmcFeatures_FeaturesId",
                table: "Mmc",
                column: "FeaturesId",
                principalTable: "MmcFeatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Ata_ATAId",
                table: "Reports",
                column: "ATAId",
                principalTable: "Ata",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Ata_ATAPIId",
                table: "Reports",
                column: "ATAPIId",
                principalTable: "Ata",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_FireWire_FireWireId",
                table: "Reports",
                column: "FireWireId",
                principalTable: "FireWire",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_MmcSd_MultiMediaCardId",
                table: "Reports",
                column: "MultiMediaCardId",
                principalTable: "MmcSd",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Pcmcia_PCMCIAId",
                table: "Reports",
                column: "PCMCIAId",
                principalTable: "Pcmcia",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Scsi_SCSIId",
                table: "Reports",
                column: "SCSIId",
                principalTable: "Scsi",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_MmcSd_SecureDigitalId",
                table: "Reports",
                column: "SecureDigitalId",
                principalTable: "MmcSd",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Usb_USBId",
                table: "Reports",
                column: "USBId",
                principalTable: "Usb",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Scsi_ScsiMode_ModeSenseId",
                table: "Scsi",
                column: "ModeSenseId",
                principalTable: "ScsiMode",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Scsi_Mmc_MultiMediaDeviceId",
                table: "Scsi",
                column: "MultiMediaDeviceId",
                principalTable: "Mmc",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Scsi_TestedMedia_ReadCapabilitiesId",
                table: "Scsi",
                column: "ReadCapabilitiesId",
                principalTable: "TestedMedia",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Scsi_Ssc_SequentialDeviceId",
                table: "Scsi",
                column: "SequentialDeviceId",
                principalTable: "Ssc",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ScsiPage_Scsi_ScsiId",
                table: "ScsiPage",
                column: "ScsiId",
                principalTable: "Scsi",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ScsiPage_ScsiMode_ScsiModeId",
                table: "ScsiPage",
                column: "ScsiModeId",
                principalTable: "ScsiMode",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_SscSupportedMedia_Ssc_SscId",
                table: "SscSupportedMedia",
                column: "SscId",
                principalTable: "Ssc",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_SscSupportedMedia_TestedSequentialMedia_TestedSequentialMedi~",
                table: "SscSupportedMedia",
                column: "TestedSequentialMediaId",
                principalTable: "TestedSequentialMedia",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportedDensity_Ssc_SscId",
                table: "SupportedDensity",
                column: "SscId",
                principalTable: "Ssc",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportedDensity_TestedSequentialMedia_TestedSequentialMedia~",
                table: "SupportedDensity",
                column: "TestedSequentialMediaId",
                principalTable: "TestedSequentialMedia",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TestedMedia_Ata_AtaId",
                table: "TestedMedia",
                column: "AtaId",
                principalTable: "Ata",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TestedMedia_Chs_CHSId",
                table: "TestedMedia",
                column: "CHSId",
                principalTable: "Chs",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TestedMedia_Chs_CurrentCHSId",
                table: "TestedMedia",
                column: "CurrentCHSId",
                principalTable: "Chs",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TestedMedia_Mmc_MmcId",
                table: "TestedMedia",
                column: "MmcId",
                principalTable: "Mmc",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TestedMedia_Scsi_ScsiId",
                table: "TestedMedia",
                column: "ScsiId",
                principalTable: "Scsi",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TestedSequentialMedia_Ssc_SscId",
                table: "TestedSequentialMedia",
                column: "SscId",
                principalTable: "Ssc",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ata_TestedMedia_ReadCapabilitiesId",
                table: "Ata");

            migrationBuilder.DropForeignKey(
                name: "FK_BlockDescriptor_ScsiMode_ScsiModeId",
                table: "BlockDescriptor");

            migrationBuilder.DropForeignKey(
                name: "FK_DensityCode_SscSupportedMedia_SscSupportedMediaId",
                table: "DensityCode");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Ata_ATAId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Ata_ATAPIId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_CdOffsets_CdOffsetId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_FireWire_FireWireId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_MmcSd_MultiMediaCardId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Pcmcia_PCMCIAId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Scsi_SCSIId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_MmcSd_SecureDigitalId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Usb_USBId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_DeviceStats_Devices_ReportId",
                table: "DeviceStats");

            migrationBuilder.DropForeignKey(
                name: "FK_Mmc_MmcFeatures_FeaturesId",
                table: "Mmc");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Ata_ATAId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Ata_ATAPIId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_FireWire_FireWireId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_MmcSd_MultiMediaCardId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Pcmcia_PCMCIAId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Scsi_SCSIId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_MmcSd_SecureDigitalId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Usb_USBId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Scsi_ScsiMode_ModeSenseId",
                table: "Scsi");

            migrationBuilder.DropForeignKey(
                name: "FK_Scsi_Mmc_MultiMediaDeviceId",
                table: "Scsi");

            migrationBuilder.DropForeignKey(
                name: "FK_Scsi_TestedMedia_ReadCapabilitiesId",
                table: "Scsi");

            migrationBuilder.DropForeignKey(
                name: "FK_Scsi_Ssc_SequentialDeviceId",
                table: "Scsi");

            migrationBuilder.DropForeignKey(
                name: "FK_ScsiPage_Scsi_ScsiId",
                table: "ScsiPage");

            migrationBuilder.DropForeignKey(
                name: "FK_ScsiPage_ScsiMode_ScsiModeId",
                table: "ScsiPage");

            migrationBuilder.DropForeignKey(
                name: "FK_SscSupportedMedia_Ssc_SscId",
                table: "SscSupportedMedia");

            migrationBuilder.DropForeignKey(
                name: "FK_SscSupportedMedia_TestedSequentialMedia_TestedSequentialMedi~",
                table: "SscSupportedMedia");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportedDensity_Ssc_SscId",
                table: "SupportedDensity");

            migrationBuilder.DropForeignKey(
                name: "FK_SupportedDensity_TestedSequentialMedia_TestedSequentialMedia~",
                table: "SupportedDensity");

            migrationBuilder.DropForeignKey(
                name: "FK_TestedMedia_Ata_AtaId",
                table: "TestedMedia");

            migrationBuilder.DropForeignKey(
                name: "FK_TestedMedia_Chs_CHSId",
                table: "TestedMedia");

            migrationBuilder.DropForeignKey(
                name: "FK_TestedMedia_Chs_CurrentCHSId",
                table: "TestedMedia");

            migrationBuilder.DropForeignKey(
                name: "FK_TestedMedia_Mmc_MmcId",
                table: "TestedMedia");

            migrationBuilder.DropForeignKey(
                name: "FK_TestedMedia_Scsi_ScsiId",
                table: "TestedMedia");

            migrationBuilder.DropForeignKey(
                name: "FK_TestedSequentialMedia_Ssc_SscId",
                table: "TestedSequentialMedia");

            migrationBuilder.AddForeignKey(
                name: "FK_Ata_TestedMedia_ReadCapabilitiesId",
                table: "Ata",
                column: "ReadCapabilitiesId",
                principalTable: "TestedMedia",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BlockDescriptor_ScsiMode_ScsiModeId",
                table: "BlockDescriptor",
                column: "ScsiModeId",
                principalTable: "ScsiMode",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DensityCode_SscSupportedMedia_SscSupportedMediaId",
                table: "DensityCode",
                column: "SscSupportedMediaId",
                principalTable: "SscSupportedMedia",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Ata_ATAId",
                table: "Devices",
                column: "ATAId",
                principalTable: "Ata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Ata_ATAPIId",
                table: "Devices",
                column: "ATAPIId",
                principalTable: "Ata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_CdOffsets_CdOffsetId",
                table: "Devices",
                column: "CdOffsetId",
                principalTable: "CdOffsets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_FireWire_FireWireId",
                table: "Devices",
                column: "FireWireId",
                principalTable: "FireWire",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_MmcSd_MultiMediaCardId",
                table: "Devices",
                column: "MultiMediaCardId",
                principalTable: "MmcSd",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Pcmcia_PCMCIAId",
                table: "Devices",
                column: "PCMCIAId",
                principalTable: "Pcmcia",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Scsi_SCSIId",
                table: "Devices",
                column: "SCSIId",
                principalTable: "Scsi",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_MmcSd_SecureDigitalId",
                table: "Devices",
                column: "SecureDigitalId",
                principalTable: "MmcSd",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Usb_USBId",
                table: "Devices",
                column: "USBId",
                principalTable: "Usb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DeviceStats_Devices_ReportId",
                table: "DeviceStats",
                column: "ReportId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mmc_MmcFeatures_FeaturesId",
                table: "Mmc",
                column: "FeaturesId",
                principalTable: "MmcFeatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Ata_ATAId",
                table: "Reports",
                column: "ATAId",
                principalTable: "Ata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Ata_ATAPIId",
                table: "Reports",
                column: "ATAPIId",
                principalTable: "Ata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_FireWire_FireWireId",
                table: "Reports",
                column: "FireWireId",
                principalTable: "FireWire",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_MmcSd_MultiMediaCardId",
                table: "Reports",
                column: "MultiMediaCardId",
                principalTable: "MmcSd",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Pcmcia_PCMCIAId",
                table: "Reports",
                column: "PCMCIAId",
                principalTable: "Pcmcia",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Scsi_SCSIId",
                table: "Reports",
                column: "SCSIId",
                principalTable: "Scsi",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_MmcSd_SecureDigitalId",
                table: "Reports",
                column: "SecureDigitalId",
                principalTable: "MmcSd",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Usb_USBId",
                table: "Reports",
                column: "USBId",
                principalTable: "Usb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Scsi_ScsiMode_ModeSenseId",
                table: "Scsi",
                column: "ModeSenseId",
                principalTable: "ScsiMode",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Scsi_Mmc_MultiMediaDeviceId",
                table: "Scsi",
                column: "MultiMediaDeviceId",
                principalTable: "Mmc",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Scsi_TestedMedia_ReadCapabilitiesId",
                table: "Scsi",
                column: "ReadCapabilitiesId",
                principalTable: "TestedMedia",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Scsi_Ssc_SequentialDeviceId",
                table: "Scsi",
                column: "SequentialDeviceId",
                principalTable: "Ssc",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ScsiPage_Scsi_ScsiId",
                table: "ScsiPage",
                column: "ScsiId",
                principalTable: "Scsi",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ScsiPage_ScsiMode_ScsiModeId",
                table: "ScsiPage",
                column: "ScsiModeId",
                principalTable: "ScsiMode",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SscSupportedMedia_Ssc_SscId",
                table: "SscSupportedMedia",
                column: "SscId",
                principalTable: "Ssc",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SscSupportedMedia_TestedSequentialMedia_TestedSequentialMedi~",
                table: "SscSupportedMedia",
                column: "TestedSequentialMediaId",
                principalTable: "TestedSequentialMedia",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportedDensity_Ssc_SscId",
                table: "SupportedDensity",
                column: "SscId",
                principalTable: "Ssc",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportedDensity_TestedSequentialMedia_TestedSequentialMedia~",
                table: "SupportedDensity",
                column: "TestedSequentialMediaId",
                principalTable: "TestedSequentialMedia",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TestedMedia_Ata_AtaId",
                table: "TestedMedia",
                column: "AtaId",
                principalTable: "Ata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TestedMedia_Chs_CHSId",
                table: "TestedMedia",
                column: "CHSId",
                principalTable: "Chs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TestedMedia_Chs_CurrentCHSId",
                table: "TestedMedia",
                column: "CurrentCHSId",
                principalTable: "Chs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TestedMedia_Mmc_MmcId",
                table: "TestedMedia",
                column: "MmcId",
                principalTable: "Mmc",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TestedMedia_Scsi_ScsiId",
                table: "TestedMedia",
                column: "ScsiId",
                principalTable: "Scsi",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TestedSequentialMedia_Ssc_SscId",
                table: "TestedSequentialMedia",
                column: "SscId",
                principalTable: "Ssc",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
