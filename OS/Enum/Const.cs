namespace NK.OS.Enum
{
    public class Const
    {
        public const int MMSYSERR_NOERROR = 0;
        public const int MAXPNAMELEN = 32;
        public const int MIXER_LONG_NAME_CHARS = 64;
        public const int MIXER_SHORT_NAME_CHARS = 16;
        public const int MIXER_GETLINEINFOF_COMPONENTTYPE = 0x3;
        public const int MIXER_GETCONTROLDETAILSF_VALUE = 0x0;
        public const int MIXER_GETLINECONTROLSF_ONEBYTYPE = 0x2;
        public const int MIXER_SETCONTROLDETAILSF_VALUE = 0x0;
        public const int MIXERLINE_COMPONENTTYPE_DST_FIRST = 0x0;
        public const int MIXERLINE_COMPONENTTYPE_SRC_FIRST = 0x1000;
        public const int MIXERLINE_COMPONENTTYPE_DST_SPEAKERS = (MIXERLINE_COMPONENTTYPE_DST_FIRST + 4);
        public const int MIXERLINE_COMPONENTTYPE_SRC_MICROPHONE = (MIXERLINE_COMPONENTTYPE_SRC_FIRST + 3);
        public const int MIXERLINE_COMPONENTTYPE_SRC_LINE = (MIXERLINE_COMPONENTTYPE_SRC_FIRST + 2);
        public const int MIXERCONTROL_CT_CLASS_FADER = 0x50000000;
        public const int MIXERCONTROL_CT_UNITS_UNSIGNED = 0x30000;
        public const int MIXERCONTROL_CONTROLTYPE_FADER = (MIXERCONTROL_CT_CLASS_FADER | MIXERCONTROL_CT_UNITS_UNSIGNED);
        public const int MIXERCONTROL_CONTROLTYPE_VOLUME = (MIXERCONTROL_CONTROLTYPE_FADER + 1);
        public const int TOKEN_ADJUST_PRIVILEGES = 0x20;
        public const int TOKEN_QUERY = 0x8;
        public const int SE_PRIVILEGE_ENABLED = 0x2;
        public const int FORMAT_MESSAGE_FROM_SYSTEM = 0x1000;
        public const int EWX_FORCE = 4;
        public const int EWX_LOGOFF = 0;
        public const int EWX_SHUTDOWN = 1;
        public const int EWX_REBOOT = 2;
        public const int EWX_POWEROFF = 8;
        public const long APPCOMMAND_VOLUME_UP = 10;
        public const uint SC_MONITORPOWER = 0xF170;
        public const int ENUM_CURRENT_SETTINGS = -1;
        public const uint DM_DISPLAYORIENTATION = 0x80;
        public const uint DM_PELSWIDTH = 0x80000;
        public const uint DM_PELSHEIGHT = 0x100000;
        public const uint DM_DISPLAYFREQUENCY = 0x400000;
        public const uint DM_BITSPERPEL = 0x40000;
    }
 
}
