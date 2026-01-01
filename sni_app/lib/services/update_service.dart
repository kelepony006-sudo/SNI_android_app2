import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:package_info_plus/package_info_plus.dart';
import '../core/constants.dart';

class UpdateService {
  static Future<bool> isUpdateAvailable() async {
    try {
      final response = await http.get(
        Uri.parse(AppConstants.updateEndpoint),
        headers: {'Accept': 'application/json'},
      );

      if (response.statusCode != 200) return false;

      final data = jsonDecode(response.body);
      final latestVersion = data['latest_version'];

      final packageInfo = await PackageInfo.fromPlatform();
      final currentVersion = packageInfo.version;

      return latestVersion != currentVersion;
    } catch (_) {
      return false;
    }
  }
}
