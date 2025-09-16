using SFA.DAS.FAA.Domain.Apply.Qualifications;

namespace SFA.DAS.FAA.Domain.UnitTests.Apply.Qualifications;

public class WhenBuildingGetAddQualificationApiRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Built(Guid qualificationReferenceId, Guid applicationId, Guid qualificationId, Guid candidateId)
    {
        var actual = new GetModifyQualificationApiRequest(qualificationReferenceId, applicationId, candidateId, qualificationId);

        actual.GetUrl.Should().Be($"applications/{applicationId}/qualifications/{qualificationReferenceId}/modify?candidateId={candidateId}&id={qualificationId}");
    }
    
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Built_With_Null_Qualification_Id(Guid qualificationReferenceId, Guid applicationId, Guid candidateId)
    {
        var actual = new GetModifyQualificationApiRequest(qualificationReferenceId, applicationId, candidateId, null);

        actual.GetUrl.Should().Be($"applications/{applicationId}/qualifications/{qualificationReferenceId}/modify?candidateId={candidateId}&id=");
    }
}