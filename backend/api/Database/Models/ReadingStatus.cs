namespace MeterVision.Api.Database.Models;

//Статус записи

public enum ReadingStatus
{
    // Ожидает подтверждения
    WaitingForConfirmation = 0,

    // Подтверждено
    Confirmed = 1,

    // Отменено
    Canceled = 2
}
