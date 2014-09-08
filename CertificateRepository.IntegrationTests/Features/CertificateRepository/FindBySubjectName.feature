Feature: FindBySubjectName
	Must throw exception when a subject name starts or ends with a left-to-right-mark (U+200E)
	Must write debug message when using FindBySubject name detailing pitfalls
	Must write debug message when subject name containers unicode characters

  @CleanupCertificatesOnError
  Scenario: Throw exeption when subject name starts with a left-to-right-mark
    Given the certificate 'sampleCertificate.pfx' has been loaded using password 'abc123' 
	  And place it into the 'My' store for the 'CurrentUser'
	When I create a certificate repository
	  And set the Store Name to 'My'
	  And set the Store Location to 'CurrentUser'
	  And I call FindBySubjectName with subject name 'Sample' prefixed with a 'left-to-right-mark'
	Then an exception will be thrown 
	  And it will be of type 'CompositeArgumentException'
	  And it will have 1 inner exception
	  And inner exception number 1 will be of type 'ArgumentException'
	  And inner exception number 1 will have a 'ParamName' set to 'thumbprint'
	Then any certificates should be cleaned up

  @CleanupCertificatesOnError
  Scenario: Throw exeption when subject name ends with a left-to-right-mark
    Given the certificate 'sampleCertificate.pfx' has been loaded using password 'abc123' 
	  And place it into the 'My' store for the 'CurrentUser'
	When I create a certificate repository
	  And set the Store Name to 'My'
	  And set the Store Location to 'CurrentUser'
	  And I call FindBySubjectName with subject name 'Sample' suffixed with a 'left-to-right-mark'
	Then an exception will be thrown 
	  And it will be of type 'CompositeArgumentException'
	  And it will have 1 inner exception
	  And inner exception number 1 will be of type 'ArgumentException'
	  And inner exception number 1 will have a 'ParamName' set to 'thumbprint'
	Then any certificates should be cleaned up

  @CleanupCertificatesOnError
  Scenario: Log debug message when calling FindBySubjectName
    Given the certificate 'sampleCertificate.pfx' has been loaded using password 'abc123' 
	  And place it into the 'My' store for the 'CurrentUser'
	  And a trace listener has been attached to the executing process
	When I create a certificate repository
	  And set the Store Name to 'My'
	  And set the Store Location to 'CurrentUser'
	  And I call FindBySubjectName with subject name 'Sample'
	Then a debug message will be logged
	Then any certificates should be cleaned up

  @CleanupCertificatesOnError
  Scenario: Log debug message when calling FindBySubjectName containing unicode characters
    Given the certificate 'sampleCertificate.pfx' has been loaded using password 'abc123' 
	  And place it into the 'My' store for the 'CurrentUser'
	  And a trace listener has been attached to the executing process
	When I create a certificate repository
	  And set the Store Name to 'My'
	  And set the Store Location to 'CurrentUser'
	  And I call FindBySubjectName with subject name 'Sample'
	Then a debug message will be logged
	Then any certificates should be cleaned up